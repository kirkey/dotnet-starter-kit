using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.EventHandlers;

/// <summary>
/// Event handler that creates an inventory transaction when a stock adjustment is created.
/// </summary>
/// <remarks>
/// This handler maintains an audit trail of all stock adjustments by creating
/// ADJUSTMENT type transactions. The transaction records the adjustment details
/// including type, reason, and quantity changes for compliance and reconciliation.
/// </remarks>
public sealed class StockAdjustmentCreatedHandler(
    ILogger<StockAdjustmentCreatedHandler> logger,
    [FromKeyedServices("store:inventory-transactions")]
    IRepository<InventoryTransaction> transactionRepository)
    : INotificationHandler<StockAdjustmentCreated>
{
    /// <summary>
    /// Handles the StockAdjustmentCreated event by creating an inventory transaction record.
    /// </summary>
    /// <param name="notification">The domain event containing the created stock adjustment.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(StockAdjustmentCreated notification, CancellationToken cancellationToken)
    {
        var adjustment = notification.StockAdjustment;

        logger.LogInformation(
            "Processing stock adjustment creation: {AdjustmentNumber} for Item {ItemId} at Warehouse {WarehouseId}: {Type}, {Quantity} units",
            adjustment.AdjustmentNumber,
            adjustment.ItemId,
            adjustment.WarehouseId,
            adjustment.AdjustmentType,
            adjustment.AdjustmentQuantity);

        try
        {
            // Determine transaction type based on adjustment type
            var (transactionType, reason) = GetTransactionDetails(adjustment);
            var transactionNumber = await GenerateTransactionNumber(cancellationToken);

            var transaction = InventoryTransaction.Create(
                transactionNumber: transactionNumber,
                itemId: adjustment.ItemId,
                warehouseId: adjustment.WarehouseId,
                warehouseLocationId: adjustment.WarehouseLocationId,
                purchaseOrderId: null,
                transactionType: transactionType,
                reason: reason,
                quantity: adjustment.AdjustmentQuantity,
                quantityBefore: adjustment.QuantityBefore,
                unitCost: adjustment.UnitCost,
                transactionDate: adjustment.AdjustmentDate,
                reference: adjustment.AdjustmentNumber,
                notes: $"Stock Adjustment {adjustment.AdjustmentNumber}: {adjustment.AdjustmentType}. Reason: {adjustment.Reason}. Before: {adjustment.QuantityBefore}, After: {adjustment.QuantityAfter}. Cost Impact: {adjustment.TotalCostImpact:C}",
                performedBy: adjustment.AdjustedBy,
                isApproved: false); // Pending approval

            await transactionRepository.AddAsync(transaction, cancellationToken);
            await transactionRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Created inventory transaction {TransactionNumber} for stock adjustment {AdjustmentNumber}",
                transactionNumber,
                adjustment.AdjustmentNumber);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to create inventory transaction for stock adjustment: {AdjustmentNumber}",
                adjustment.AdjustmentNumber);
            // Don't rethrow - event handlers should not break the main flow
        }
    }

    /// <summary>
    /// Determines transaction type and reason based on the adjustment type.
    /// </summary>
    private static (string TransactionType, string Reason) GetTransactionDetails(StockAdjustment adjustment)
    {
        return adjustment.AdjustmentType switch
        {
            "Increase" => ("IN", "ADJUSTMENT_INCREASE"),
            "Found" => ("IN", "ADJUSTMENT_FOUND"),
            "Decrease" => ("OUT", "ADJUSTMENT_DECREASE"),
            "Write-Off" => ("OUT", "ADJUSTMENT_WRITEOFF"),
            "Damage" => ("OUT", "ADJUSTMENT_DAMAGE"),
            "Loss" => ("OUT", "ADJUSTMENT_LOSS"),
            "Physical Count" => (adjustment.IsStockIncrease() ? "IN" : "OUT", "ADJUSTMENT_PHYSICAL_COUNT"),
            _ => ("ADJUSTMENT", $"ADJUSTMENT_{adjustment.AdjustmentType.ToUpperInvariant().Replace(" ", "_")}")
        };
    }

    /// <summary>
    /// Generates a unique transaction number.
    /// </summary>
    private async Task<string> GenerateTransactionNumber(CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var prefix = $"TXN-ADJ-{date:yyyyMMdd}";
        
        // Get count of transactions
        var count = await transactionRepository.CountAsync(cancellationToken);
        
        return $"{prefix}-{(count + 1):D6}";
    }
}

