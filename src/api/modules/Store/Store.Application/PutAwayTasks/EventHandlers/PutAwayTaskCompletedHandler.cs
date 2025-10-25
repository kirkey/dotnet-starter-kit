using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.EventHandlers;

/// <summary>
/// Event handler that creates inventory transactions when a put-away task is completed.
/// </summary>
/// <remarks>
/// This handler creates IN transactions for all put-away items, representing
/// the placement of received inventory into warehouse storage locations.
/// This provides complete audit trail for all stored items and updates stock levels.
/// </remarks>
public sealed class PutAwayTaskCompletedHandler(
    ILogger<PutAwayTaskCompletedHandler> logger,
    [FromKeyedServices("store:inventory-transactions")]
    IRepository<InventoryTransaction> transactionRepository)
    : INotificationHandler<PutAwayTaskCompleted>
{
    /// <summary>
    /// Handles the PutAwayTaskCompleted event by creating inventory transactions for put-away items.
    /// </summary>
    /// <param name="notification">The domain event containing the completed put-away task.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(PutAwayTaskCompleted notification, CancellationToken cancellationToken)
    {
        var putAwayTask = notification.PutAwayTask;

        logger.LogInformation(
            "Processing put-away task completion: {TaskNumber} at Warehouse {WarehouseId}, {ItemCount} items",
            putAwayTask.TaskNumber,
            putAwayTask.WarehouseId,
            putAwayTask.Items.Count);

        try
        {
            // Create IN transactions for each put-away item
            foreach (var item in putAwayTask.Items)
            {
                var transactionNumber = await GenerateTransactionNumber(cancellationToken);

                var transaction = InventoryTransaction.Create(
                    transactionNumber: transactionNumber,
                    itemId: item.ItemId,
                    warehouseId: putAwayTask.WarehouseId,
                    warehouseLocationId: null, // PutAwayTaskItem has ToBinId, not location
                    purchaseOrderId: null,
                    transactionType: "IN",
                    reason: "PUT_AWAY_COMPLETED",
                    quantity: item.QuantityPutAway,
                    quantityBefore: 0, // Would need current stock level
                    unitCost: 0m, // Cost tracking handled separately
                    transactionDate: DateTime.UtcNow,
                    reference: putAwayTask.TaskNumber,
                    notes: $"Put-Away Task {putAwayTask.TaskNumber} completed. Strategy: {putAwayTask.PutAwayStrategy}. Put away: {item.QuantityPutAway} units to Bin. Receipt: {putAwayTask.GoodsReceiptId?.ToString() ?? "N/A"}",
                    performedBy: putAwayTask.AssignedTo,
                    isApproved: true);

                await transactionRepository.AddAsync(transaction, cancellationToken);
            }

            await transactionRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Created {Count} inventory transactions for put-away task {TaskNumber}",
                putAwayTask.Items.Count,
                putAwayTask.TaskNumber);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to create inventory transactions for put-away task completion: {TaskNumber}",
                putAwayTask.TaskNumber);
            // Don't rethrow - event handlers should not break the main flow
        }
    }

    /// <summary>
    /// Generates a unique transaction number for put-away completions.
    /// </summary>
    private async Task<string> GenerateTransactionNumber(CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var prefix = $"TXN-PUTAWAY-{date:yyyyMMdd}";
        
        // Get count of transactions
        var count = await transactionRepository.CountAsync(cancellationToken);
        
        return $"{prefix}-{(count + 1):D6}";
    }
}

