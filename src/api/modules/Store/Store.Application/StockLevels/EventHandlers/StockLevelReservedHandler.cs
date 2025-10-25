using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.StockLevels.EventHandlers;

/// <summary>
/// Event handler that creates an inventory transaction when stock is reserved.
/// </summary>
/// <remarks>
/// This handler maintains an audit trail of all stock reservations by creating
/// ADJUSTMENT type transactions with reason "RESERVED". This provides traceability
/// for inventory allocation and helps with inventory reconciliation.
/// </remarks>
public sealed class StockLevelReservedHandler(
    ILogger<StockLevelReservedHandler> logger,
    [FromKeyedServices("store:inventory-transactions")]
    IRepository<InventoryTransaction> transactionRepository,
    [FromKeyedServices("store:stocklevels")]
    IReadRepository<StockLevel> stockLevelRepository)
    : INotificationHandler<StockLevelReserved>
{
    private readonly IReadRepository<StockLevel> _stockLevelRepository = stockLevelRepository;

    /// <summary>
    /// Handles the StockLevelReserved event by creating an inventory transaction record.
    /// </summary>
    /// <param name="notification">The domain event containing the stock level with reserved quantity.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(StockLevelReserved notification, CancellationToken cancellationToken)
    {
        var stockLevel = notification.StockLevel;

        logger.LogInformation(
            "Processing stock reservation for Item {ItemId} at Warehouse {WarehouseId}: {Quantity} units",
            stockLevel.ItemId,
            stockLevel.WarehouseId,
            notification.ReservedQuantity);

        try
        {
            // Create an ADJUSTMENT transaction to record the reservation
            var transactionNumber = await GenerateTransactionNumber(cancellationToken);
            var quantityBefore = stockLevel.QuantityAvailable + notification.ReservedQuantity;

            var transaction = InventoryTransaction.Create(
                transactionNumber: transactionNumber,
                itemId: stockLevel.ItemId,
                warehouseId: stockLevel.WarehouseId,
                warehouseLocationId: stockLevel.WarehouseLocationId,
                purchaseOrderId: null,
                transactionType: "ADJUSTMENT",
                reason: "RESERVED",
                quantity: notification.ReservedQuantity,
                quantityBefore: quantityBefore,
                unitCost: 0m, // Reservation doesn't affect cost
                transactionDate: DateTime.UtcNow,
                reference: $"Reserved from Stock Level {stockLevel.Id}",
                notes: $"Reserved {notification.ReservedQuantity} units. Available: {stockLevel.QuantityAvailable}, Reserved: {stockLevel.QuantityReserved}",
                performedBy: null,
                isApproved: true);

            await transactionRepository.AddAsync(transaction, cancellationToken);
            await transactionRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Created inventory transaction {TransactionNumber} for stock reservation",
                transactionNumber);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to create inventory transaction for stock reservation: Item {ItemId}, Warehouse {WarehouseId}",
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
        var prefix = $"TXN-RES-{date:yyyyMMdd}";
        
        // Get count of transactions with this prefix
        var count = await transactionRepository.CountAsync(cancellationToken);
        
        return $"{prefix}-{(count + 1):D6}";
    }
}
