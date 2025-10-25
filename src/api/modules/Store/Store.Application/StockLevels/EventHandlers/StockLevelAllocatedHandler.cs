using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.StockLevels.EventHandlers;

/// <summary>
/// Event handler that creates an inventory transaction when stock is allocated to pick lists.
/// </summary>
/// <remarks>
/// This handler maintains an audit trail of all stock allocations by creating
/// ADJUSTMENT type transactions with reason "ALLOCATED". This provides traceability
/// for pick list assignments and helps with inventory reconciliation.
/// </remarks>
public sealed class StockLevelAllocatedHandler(
    ILogger<StockLevelAllocatedHandler> logger,
    [FromKeyedServices("store:inventory-transactions")]
    IRepository<InventoryTransaction> transactionRepository,
    [FromKeyedServices("store:stocklevels")]
    IReadRepository<StockLevel> stockLevelRepository)
    : INotificationHandler<StockLevelAllocated>
{
    private readonly IReadRepository<StockLevel> _stockLevelRepository = stockLevelRepository;

    /// <summary>
    /// Handles the StockLevelAllocated event by creating an inventory transaction record.
    /// </summary>
    /// <param name="notification">The domain event containing the stock level with allocated quantity.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(StockLevelAllocated notification, CancellationToken cancellationToken)
    {
        var stockLevel = notification.StockLevel;

        logger.LogInformation(
            "Processing stock allocation for Item {ItemId} at Warehouse {WarehouseId}: {Quantity} units",
            stockLevel.ItemId,
            stockLevel.WarehouseId,
            notification.AllocatedQuantity);

        try
        {
            // Create an ADJUSTMENT transaction to record the allocation
            var transactionNumber = await GenerateTransactionNumber(cancellationToken);
            var quantityBefore = stockLevel.QuantityReserved + notification.AllocatedQuantity;

            var transaction = InventoryTransaction.Create(
                transactionNumber: transactionNumber,
                itemId: stockLevel.ItemId,
                warehouseId: stockLevel.WarehouseId,
                warehouseLocationId: stockLevel.WarehouseLocationId,
                purchaseOrderId: null,
                transactionType: "ADJUSTMENT",
                reason: "ALLOCATED",
                quantity: notification.AllocatedQuantity,
                quantityBefore: quantityBefore,
                unitCost: 0m, // Allocation doesn't affect cost
                transactionDate: DateTime.UtcNow,
                reference: $"Allocated from Stock Level {stockLevel.Id}",
                notes: $"Allocated {notification.AllocatedQuantity} units to pick list. Reserved: {stockLevel.QuantityReserved}, Allocated: {stockLevel.QuantityAllocated}",
                performedBy: null,
                isApproved: true);

            await transactionRepository.AddAsync(transaction, cancellationToken);
            await transactionRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Created inventory transaction {TransactionNumber} for stock allocation",
                transactionNumber);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to create inventory transaction for stock allocation: Item {ItemId}, Warehouse {WarehouseId}",
                stockLevel.ItemId,
                stockLevel.WarehouseId);
            // Don't rethrow - event handlers should not break the main flow
        }
    }

    /// <summary>
    /// Generates a unique transaction number.
    /// </summary>
    private async Task<string> GenerateTransactionNumber(CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var prefix = $"TXN-ALC-{date:yyyyMMdd}";
        
        // Get count of transactions with this prefix
        var count = await transactionRepository.CountAsync(cancellationToken);
        
        return $"{prefix}-{(count + 1):D6}";
    }
}
