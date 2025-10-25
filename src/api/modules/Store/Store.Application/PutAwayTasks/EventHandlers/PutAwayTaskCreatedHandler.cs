using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.EventHandlers;

/// <summary>
/// Event handler that creates inventory transactions when a put-away task is created.
/// </summary>
/// <remarks>
/// This handler maintains an audit trail of put-away task creation by creating
/// ADJUSTMENT type transactions with reason "PUT_AWAY_CREATED". This provides
/// visibility into when put-away tasks are initiated for received goods.
/// </remarks>
public sealed class PutAwayTaskCreatedHandler(
    ILogger<PutAwayTaskCreatedHandler> logger,
    [FromKeyedServices("store:inventory-transactions")]
    IRepository<InventoryTransaction> transactionRepository)
    : INotificationHandler<PutAwayTaskCreated>
{
    /// <summary>
    /// Handles the PutAwayTaskCreated event by creating inventory transaction records.
    /// </summary>
    /// <param name="notification">The domain event containing the created put-away task.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(PutAwayTaskCreated notification, CancellationToken cancellationToken)
    {
        var putAwayTask = notification.PutAwayTask;

        logger.LogInformation(
            "Processing put-away task creation: {TaskNumber} at Warehouse {WarehouseId}, Strategy: {Strategy}",
            putAwayTask.TaskNumber,
            putAwayTask.WarehouseId,
            putAwayTask.PutAwayStrategy);

        try
        {
            // Create adjustment transactions for each item to record put-away task creation
            foreach (var item in putAwayTask.Items)
            {
                var transactionNumber = await GenerateTransactionNumber(cancellationToken);

                var transaction = InventoryTransaction.Create(
                    transactionNumber: transactionNumber,
                    itemId: item.ItemId,
                    warehouseId: putAwayTask.WarehouseId,
                    warehouseLocationId: null, // PutAwayTaskItem has ToBinId, not location
                    purchaseOrderId: null,
                    transactionType: "ADJUSTMENT",
                    reason: "PUT_AWAY_CREATED",
                    quantity: item.QuantityToPutAway,
                    quantityBefore: 0,
                    unitCost: 0m,
                    transactionDate: DateTime.UtcNow,
                    reference: putAwayTask.TaskNumber,
                    notes: $"Put-Away Task {putAwayTask.TaskNumber} created. Strategy: {putAwayTask.PutAwayStrategy}, Priority: {putAwayTask.Priority}. Receipt: {putAwayTask.GoodsReceiptId?.ToString() ?? "N/A"}",
                    performedBy: null,
                    isApproved: false); // Pending completion

                await transactionRepository.AddAsync(transaction, cancellationToken);
            }

            await transactionRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Created {Count} inventory transactions for put-away task creation {TaskNumber}",
                putAwayTask.Items.Count,
                putAwayTask.TaskNumber);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to create inventory transactions for put-away task creation: {TaskNumber}",
                putAwayTask.TaskNumber);
            // Don't rethrow - event handlers should not break the main flow
        }
    }

    /// <summary>
    /// Generates a unique transaction number for put-away task created transactions.
    /// </summary>
    private async Task<string> GenerateTransactionNumber(CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var prefix = $"TXN-PACR-{date:yyyyMMdd}";
        
        // Get count of transactions
        var count = await transactionRepository.CountAsync(cancellationToken);
        
        return $"{prefix}-{(count + 1):D6}";
    }
}

