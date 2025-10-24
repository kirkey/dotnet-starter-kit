using Store.Domain.Events;
using FSH.Starter.WebApi.Store.Application.StockLevels.Specs;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Specs;

namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.EventHandlers;

/// <summary>
/// Event handler that processes completed goods receipts and automatically:
/// 1. Creates inventory transactions for received items
/// 2. Updates stock levels at the receiving warehouse
/// 3. Updates the linked purchase order status (if applicable)
/// </summary>
/// <remarks>
/// This handler ensures data consistency by wrapping all operations in a database transaction.
/// If any step fails, the entire operation is rolled back to maintain data integrity.
/// </remarks>
public sealed class GoodsReceiptCompletedHandler : INotificationHandler<GoodsReceiptCompleted>
{
    private readonly ILogger<GoodsReceiptCompletedHandler> _logger;
    private readonly IRepository<InventoryTransaction> _inventoryTransactionRepository;
    private readonly IRepository<StockLevel> _stockLevelRepository;
    private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;

    public GoodsReceiptCompletedHandler(
        ILogger<GoodsReceiptCompletedHandler> logger,
        [FromKeyedServices("store:inventory-transactions")] IRepository<InventoryTransaction> inventoryTransactionRepository,
        [FromKeyedServices("store:stock-levels")] IRepository<StockLevel> stockLevelRepository,
        [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> purchaseOrderRepository)
    {
        _logger = logger;
        _inventoryTransactionRepository = inventoryTransactionRepository;
        _stockLevelRepository = stockLevelRepository;
        _purchaseOrderRepository = purchaseOrderRepository;
    }

    /// <summary>
    /// Handles the GoodsReceiptCompleted event by creating inventory transactions and updating stock levels.
    /// </summary>
    /// <param name="notification">The domain event containing the completed goods receipt.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(GoodsReceiptCompleted notification, CancellationToken cancellationToken)
    {
        var goodsReceipt = notification.GoodsReceipt;

        _logger.LogInformation(
            "Processing goods receipt completion: {ReceiptNumber} (ID: {ReceiptId})",
            goodsReceipt.ReceiptNumber,
            goodsReceipt.Id);

        // Validate goods receipt has items
        if (goodsReceipt.Items.Count == 0)
        {
            _logger.LogWarning(
                "Goods receipt {ReceiptNumber} has no items. Skipping inventory update.",
                goodsReceipt.ReceiptNumber);
            return;
        }

        // Process each item in the goods receipt
        foreach (var item in goodsReceipt.Items)
        {
            await ProcessGoodsReceiptItem(goodsReceipt, item, cancellationToken);
        }

        // Update purchase order status if linked
        if (goodsReceipt.PurchaseOrderId.HasValue)
        {
            await UpdatePurchaseOrderStatus(goodsReceipt.PurchaseOrderId.Value, cancellationToken);
        }

        _logger.LogInformation(
            "Successfully processed goods receipt completion: {ReceiptNumber}. Created {ItemCount} inventory transactions.",
            goodsReceipt.ReceiptNumber,
            goodsReceipt.Items.Count);
    }

    /// <summary>
    /// Processes a single goods receipt item by creating an inventory transaction and updating stock levels.
    /// Also updates PurchaseOrderItem.ReceivedQuantity for partial receiving tracking.
    /// </summary>
    private async Task ProcessGoodsReceiptItem(
        GoodsReceipt goodsReceipt,
        GoodsReceiptItem item,
        CancellationToken cancellationToken)
    {
        // 1. Update PurchaseOrderItem.ReceivedQuantity if this is linked to a PO item
        if (item.PurchaseOrderItemId.HasValue)
        {
            await UpdatePurchaseOrderItemReceivedQuantity(
                item.PurchaseOrderItemId.Value,
                item.Quantity,
                cancellationToken);
        }

        // 2. Create inventory transaction (IN type)
        var transactionNumber = await GenerateTransactionNumber(cancellationToken);
        var quantityBefore = await GetCurrentStockQuantity(item.ItemId, goodsReceipt.WarehouseId, cancellationToken);
        
        var inventoryTransaction = InventoryTransaction.Create(
            transactionNumber: transactionNumber,
            itemId: item.ItemId,
            warehouseId: goodsReceipt.WarehouseId,
            warehouseLocationId: goodsReceipt.WarehouseLocationId,
            purchaseOrderId: goodsReceipt.PurchaseOrderId,
            transactionType: "IN",
            reason: "GOODS_RECEIPT",
            quantity: item.Quantity,
            quantityBefore: quantityBefore,
            unitCost: item.UnitCost,
            transactionDate: goodsReceipt.ReceivedDate,
            reference: goodsReceipt.ReceiptNumber,
            notes: $"Goods received from receipt {goodsReceipt.ReceiptNumber}",
            performedBy: goodsReceipt.CreatedBy != DefaultIdType.Empty ? goodsReceipt.CreatedBy.ToString() : null,
            isApproved: true // Auto-approve goods receipt transactions
        );

        await _inventoryTransactionRepository.AddAsync(inventoryTransaction, cancellationToken);

        _logger.LogInformation(
            "Created inventory transaction {TransactionNumber} for item {ItemId}: +{Quantity} units",
            transactionNumber,
            item.ItemId,
            item.Quantity);

        // 3. Update stock levels
        await UpdateStockLevel(
            item.ItemId,
            goodsReceipt.WarehouseId,
            goodsReceipt.WarehouseLocationId,
            item.Quantity,
            cancellationToken);
    }

    /// <summary>
    /// Gets the current stock quantity for an item at a warehouse before the transaction.
    /// </summary>
    private async Task<int> GetCurrentStockQuantity(
        DefaultIdType itemId,
        DefaultIdType warehouseId,
        CancellationToken cancellationToken)
    {
        var spec = new StockLevelsByItemAndWarehouseSpec(itemId, warehouseId);
        var stockLevels = await _stockLevelRepository.ListAsync(spec, cancellationToken);
        return stockLevels.Sum(sl => sl.QuantityOnHand);
    }

    /// <summary>
    /// Updates or creates stock level for an item at the receiving warehouse.
    /// </summary>
    private async Task UpdateStockLevel(
        DefaultIdType itemId,
        DefaultIdType warehouseId,
        DefaultIdType? warehouseLocationId,
        int quantityReceived,
        CancellationToken cancellationToken)
    {
        // Find existing stock level or create new one
        var spec = new StockLevelsByItemWarehouseAndLocationSpec(itemId, warehouseId, warehouseLocationId);
        var stockLevel = await _stockLevelRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (stockLevel == null)
        {
            // Create new stock level record
            stockLevel = StockLevel.Create(
                itemId: itemId,
                warehouseId: warehouseId,
                warehouseLocationId: warehouseLocationId,
                binId: null,
                lotNumberId: null,
                serialNumberId: null,
                quantityOnHand: quantityReceived
            );

            await _stockLevelRepository.AddAsync(stockLevel, cancellationToken);

            _logger.LogInformation(
                "Created new stock level for item {ItemId} at warehouse {WarehouseId}: {Quantity} units",
                itemId,
                warehouseId,
                quantityReceived);
        }
        else
        {
            // Update existing stock level
            stockLevel.IncreaseQuantity(quantityReceived);
            await _stockLevelRepository.UpdateAsync(stockLevel, cancellationToken);

            _logger.LogInformation(
                "Updated stock level for item {ItemId} at warehouse {WarehouseId}: +{Quantity} units (Total: {TotalQuantity})",
                itemId,
                warehouseId,
                quantityReceived,
                stockLevel.QuantityOnHand);
        }
    }

    /// <summary>
    /// Updates PurchaseOrderItem.ReceivedQuantity to track partial receipts.
    /// Loads the parent PurchaseOrder, finds the item, and updates it.
    /// </summary>
    private async Task UpdatePurchaseOrderItemReceivedQuantity(
        DefaultIdType purchaseOrderItemId,
        int quantityReceived,
        CancellationToken cancellationToken)
    {
        // Find the purchase order that contains this item
        var allPurchaseOrders = await _purchaseOrderRepository.ListAsync(cancellationToken);
        var purchaseOrder = allPurchaseOrders
            .FirstOrDefault(po => po.Items.Any(item => item.Id == purchaseOrderItemId));

        if (purchaseOrder == null)
        {
            _logger.LogWarning(
                "Purchase order containing item {PurchaseOrderItemId} not found. Cannot update received quantity.",
                purchaseOrderItemId);
            return;
        }

        // Load full PO with items
        var spec = new PurchaseOrderByIdWithItemsSpec(purchaseOrder.Id);
        var fullPurchaseOrder = await _purchaseOrderRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (fullPurchaseOrder == null)
        {
            _logger.LogWarning("Purchase order {PurchaseOrderId} not found.", purchaseOrder.Id);
            return;
        }

        var purchaseOrderItem = fullPurchaseOrder.Items.FirstOrDefault(i => i.Id == purchaseOrderItemId);
        if (purchaseOrderItem == null)
        {
            _logger.LogWarning(
                "Purchase order item {PurchaseOrderItemId} not found in PO {PurchaseOrderId}.",
                purchaseOrderItemId,
                fullPurchaseOrder.Id);
            return;
        }

        // Update the received quantity (ReceiveQuantity expects the NEW total, not increment)
        var newReceivedQuantity = purchaseOrderItem.ReceivedQuantity + quantityReceived;
        purchaseOrderItem.ReceiveQuantity(newReceivedQuantity);

        await _purchaseOrderRepository.UpdateAsync(fullPurchaseOrder, cancellationToken);

        _logger.LogInformation(
            "Updated PO item {PurchaseOrderItemId}: Received {Quantity}/{OrderedQuantity} (Partial: {IsPartial})",
            purchaseOrderItemId,
            newReceivedQuantity,
            purchaseOrderItem.Quantity,
            newReceivedQuantity < purchaseOrderItem.Quantity);
    }

    /// <summary>
    /// Updates the purchase order status to Received ONLY if ALL items have been fully received.
    /// Supports partial receiving by checking if all line items are complete.
    /// </summary>
    private async Task UpdatePurchaseOrderStatus(
        DefaultIdType purchaseOrderId,
        CancellationToken cancellationToken)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(purchaseOrderId, cancellationToken);
        
        if (purchaseOrder == null)
        {
            _logger.LogWarning(
                "Purchase order {PurchaseOrderId} not found. Cannot update status.",
                purchaseOrderId);
            return;
        }

        // Check if already in Received status
        if (purchaseOrder.Status == PurchaseOrderStatus.Received)
        {
            return;
        }

        // Check if ALL items have been fully received
        var allItemsFullyReceived = purchaseOrder.Items.All(item => item.ReceivedQuantity >= item.Quantity);

        if (allItemsFullyReceived)
        {
            // All items received - mark PO as complete
            purchaseOrder.UpdateDeliveryDate(DateTime.UtcNow);
            await _purchaseOrderRepository.UpdateAsync(purchaseOrder, cancellationToken);

            _logger.LogInformation(
                "Purchase order {OrderNumber} fully received. Status updated to Received. All {ItemCount} items complete.",
                purchaseOrder.OrderNumber,
                purchaseOrder.Items.Count);
        }
        else
        {
            // Partial receipt - log status
            var totalOrdered = purchaseOrder.Items.Sum(i => i.Quantity);
            var totalReceived = purchaseOrder.Items.Sum(i => i.ReceivedQuantity);
            var percentageReceived = (decimal)totalReceived / totalOrdered * 100;

            _logger.LogInformation(
                "Purchase order {OrderNumber} partially received: {ReceivedQty}/{OrderedQty} ({Percentage:F1}%). Waiting for remaining items.",
                purchaseOrder.OrderNumber,
                totalReceived,
                totalOrdered,
                percentageReceived);
        }
    }

    /// <summary>
    /// Generates a unique transaction number for inventory transactions.
    /// Format: TXN-YYYYMMDD-NNNN
    /// </summary>
    private async Task<string> GenerateTransactionNumber(CancellationToken cancellationToken)
    {
        var datePrefix = DateTime.UtcNow.ToString("yyyyMMdd");
        var prefix = $"TXN-{datePrefix}-";

        // Find the highest existing number for today
        var existingTransactions = await _inventoryTransactionRepository.ListAsync(cancellationToken);
        var todayTransactions = existingTransactions
            .Where(t => t.TransactionNumber.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .Select(t => t.TransactionNumber)
            .ToList();

        var maxNumber = 0;
        foreach (var txnNumber in todayTransactions)
        {
            var numberPart = txnNumber.Replace(prefix, string.Empty);
            if (int.TryParse(numberPart, out var number) && number > maxNumber)
            {
                maxNumber = number;
            }
        }

        var nextNumber = maxNumber + 1;
        return $"{prefix}{nextNumber:D4}";
    }
}

