using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.StockLevels.EventHandlers;

/// <summary>
/// Event handler that creates an inventory transaction when stock levels are updated.
/// </summary>
/// <remarks>
/// This handler maintains an audit trail of stock level changes including increases,
/// decreases, releases, and picks by creating appropriate inventory transactions.
/// This provides complete traceability for all inventory movements.
/// </remarks>
public sealed class StockLevelUpdatedHandler : INotificationHandler<StockLevelUpdated>
{
    private readonly ILogger<StockLevelUpdatedHandler> _logger;
    private readonly IRepository<InventoryTransaction> _transactionRepository;

    public StockLevelUpdatedHandler(
        ILogger<StockLevelUpdatedHandler> logger,
        [FromKeyedServices("store:inventory-transactions")] IRepository<InventoryTransaction> transactionRepository)
    {
        _logger = logger;
        _transactionRepository = transactionRepository;
    }

    /// <summary>
    /// Handles the StockLevelUpdated event by creating an inventory transaction record.
    /// </summary>
    /// <param name="notification">The domain event containing the updated stock level and change details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(StockLevelUpdated notification, CancellationToken cancellationToken)
    {
        var stockLevel = notification.StockLevel;
        var changeType = notification.ChangeType;

        // Only create transactions for actual quantity changes, not for location updates
        if (changeType == "LOCATION_UPDATE")
        {
            _logger.LogDebug(
                "Skipping transaction creation for location update on Stock Level {StockLevelId}",
                stockLevel.Id);
            return;
        }

        _logger.LogInformation(
            "Processing stock level update for Item {ItemId} at Warehouse {WarehouseId}: {ChangeType}, Change: {QuantityChange}",
            stockLevel.ItemId,
            stockLevel.WarehouseId,
            changeType,
            notification.QuantityChange);

        try
        {
            var transactionNumber = await GenerateTransactionNumber(changeType, cancellationToken);
            var (transactionType, reason) = GetTransactionDetails(changeType);
            var quantityBefore = stockLevel.QuantityOnHand - notification.QuantityChange;

            var transaction = InventoryTransaction.Create(
                transactionNumber: transactionNumber,
                itemId: stockLevel.ItemId,
                warehouseId: stockLevel.WarehouseId,
                warehouseLocationId: stockLevel.WarehouseLocationId,
                purchaseOrderId: null,
                transactionType: transactionType,
                reason: reason,
                quantity: Math.Abs(notification.QuantityChange),
                quantityBefore: Math.Max(0, quantityBefore),
                unitCost: 0m, // Cost tracking handled separately
                transactionDate: DateTime.UtcNow,
                reference: $"Stock Level {stockLevel.Id}",
                notes: $"{changeType}: {notification.QuantityChange} units. OnHand: {stockLevel.QuantityOnHand}, Available: {stockLevel.QuantityAvailable}",
                performedBy: null,
                isApproved: true);

            await _transactionRepository.AddAsync(transaction, cancellationToken);
            await _transactionRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Created inventory transaction {TransactionNumber} for stock level update: {ChangeType}",
                transactionNumber,
                changeType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to create inventory transaction for stock level update: Item {ItemId}, Warehouse {WarehouseId}, ChangeType {ChangeType}",
                stockLevel.ItemId,
                stockLevel.WarehouseId,
                changeType);
            // Don't rethrow - event handlers should not break the main flow
        }
    }

    /// <summary>
    /// Determines transaction type and reason based on the change type.
    /// </summary>
    private static (string TransactionType, string Reason) GetTransactionDetails(string changeType)
    {
        return changeType switch
        {
            "INCREASE" => ("IN", "STOCK_INCREASE"),
            "DECREASE" => ("OUT", "STOCK_DECREASE"),
            "RELEASE_RESERVATION" => ("ADJUSTMENT", "RESERVATION_RELEASED"),
            "PICK_CONFIRMED" => ("OUT", "PICK_CONFIRMED"),
            _ => ("ADJUSTMENT", changeType)
        };
    }

    /// <summary>
    /// Generates a unique transaction number based on the change type.
    /// </summary>
    private async Task<string> GenerateTransactionNumber(string changeType, CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var typePrefix = changeType switch
        {
            "INCREASE" => "INC",
            "DECREASE" => "DEC",
            "RELEASE_RESERVATION" => "REL",
            "PICK_CONFIRMED" => "PCK",
            _ => "ADJ"
        };
        
        var prefix = $"TXN-{typePrefix}-{date:yyyyMMdd}";
        
        // Get count of transactions with this prefix
        var count = await _transactionRepository.CountAsync(cancellationToken);
        
        return $"{prefix}-{(count + 1):D6}";
    }
}

