using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.PickLists.EventHandlers;

/// <summary>
/// Event handler that creates inventory transactions when a pick list is cancelled.
/// </summary>
/// <remarks>
/// This handler records the cancellation event in the audit trail with ADJUSTMENT
/// transactions. This tracks when pick lists are cancelled before completion,
/// releasing reserved/allocated inventory back to available.
/// </remarks>
public sealed class PickListCancelledHandler(
    ILogger<PickListCancelledHandler> logger,
    [FromKeyedServices("store:inventory-transactions")] IRepository<InventoryTransaction> transactionRepository) : INotificationHandler<PickListCancelled>
{
    /// <summary>
    /// Handles the PickListCancelled event by creating inventory transaction records.
    /// </summary>
    /// <param name="notification">The domain event containing the cancelled pick list.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(PickListCancelled notification, CancellationToken cancellationToken)
    {
        var pickList = notification.PickList;

        logger.LogInformation(
            "Processing pick list cancellation: {PickListNumber}, Reason: {Reason}",
            pickList.PickListNumber,
            notification.Reason);

        try
        {
            // Create adjustment transactions for each item to record cancellation
            foreach (var item in pickList.Items)
            {
                var transactionNumber = await GenerateTransactionNumber(cancellationToken);

                var transaction = InventoryTransaction.Create(
                    transactionNumber: transactionNumber,
                    itemId: item.ItemId,
                    warehouseId: pickList.WarehouseId,
                    warehouseLocationId: null, // PickList item has BinId, not location
                    purchaseOrderId: null,
                    transactionType: "ADJUSTMENT",
                    reason: "PICK_LIST_CANCELLED",
                    quantity: item.QuantityToPick,
                    quantityBefore: 0,
                    unitCost: 0m,
                    transactionDate: DateTime.UtcNow,
                    reference: pickList.PickListNumber,
                    notes: $"Pick List {pickList.PickListNumber} cancelled. Reason: {notification.Reason}",
                    performedBy: pickList.AssignedTo,
                    isApproved: true);

                await transactionRepository.AddAsync(transaction, cancellationToken);
            }

            await transactionRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Created {Count} inventory transactions for pick list cancellation {PickListNumber}",
                pickList.Items.Count,
                pickList.PickListNumber);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to create inventory transactions for pick list cancellation: {PickListNumber}",
                pickList.PickListNumber);
            // Don't rethrow - event handlers should not break the main flow
        }
    }

    /// <summary>
    /// Generates a unique transaction number for pick list cancelled transactions.
    /// </summary>
    private async Task<string> GenerateTransactionNumber(CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var prefix = $"TXN-PICKCN-{date:yyyyMMdd}";
        
        // Get count of transactions
        var count = await transactionRepository.CountAsync(cancellationToken);
        
        return $"{prefix}-{(count + 1):D6}";
    }
}

