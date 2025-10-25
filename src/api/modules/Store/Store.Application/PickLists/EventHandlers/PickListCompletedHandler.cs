using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.PickLists.EventHandlers;

/// <summary>
/// Event handler that creates inventory transactions when a pick list is completed.
/// </summary>
/// <remarks>
/// This handler creates OUT transactions for all picked items, representing
/// the removal of inventory from warehouse locations for order fulfillment.
/// This provides complete audit trail for all picked items.
/// </remarks>
public sealed class PickListCompletedHandler(
    ILogger<PickListCompletedHandler> logger,
    [FromKeyedServices("store:inventory-transactions")] IRepository<InventoryTransaction> transactionRepository) : INotificationHandler<PickListCompleted>
{
    /// <summary>
    /// Handles the PickListCompleted event by creating inventory transactions for picked items.
    /// </summary>
    /// <param name="notification">The domain event containing the completed pick list.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(PickListCompleted notification, CancellationToken cancellationToken)
    {
        var pickList = notification.PickList;

        logger.LogInformation(
            "Processing pick list completion: {PickListNumber} at Warehouse {WarehouseId}, {ItemCount} items",
            pickList.PickListNumber,
            pickList.WarehouseId,
            pickList.Items.Count);

        try
        {
            // Create OUT transactions for each picked item
            foreach (var item in pickList.Items)
            {
                var transactionNumber = await GenerateTransactionNumber(cancellationToken);

                var transaction = InventoryTransaction.Create(
                    transactionNumber: transactionNumber,
                    itemId: item.ItemId,
                    warehouseId: pickList.WarehouseId,
                    warehouseLocationId: null, // PickList item has BinId, not location
                    purchaseOrderId: null,
                    transactionType: "OUT",
                    reason: "PICK_COMPLETED",
                    quantity: item.QuantityPicked,
                    quantityBefore: 0, // Would need current stock level
                    unitCost: 0m, // Cost tracking handled separately
                    transactionDate: DateTime.UtcNow,
                    reference: pickList.PickListNumber,
                    notes: $"Pick List {pickList.PickListNumber} completed. Type: {pickList.PickingType}. Picked: {item.QuantityPicked} units. Reference: {pickList.ReferenceNumber ?? "N/A"}",
                    performedBy: pickList.AssignedTo,
                    isApproved: true);

                await transactionRepository.AddAsync(transaction, cancellationToken);
            }

            await transactionRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Created {Count} inventory transactions for pick list {PickListNumber}",
                pickList.Items.Count,
                pickList.PickListNumber);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to create inventory transactions for pick list completion: {PickListNumber}",
                pickList.PickListNumber);
            // Don't rethrow - event handlers should not break the main flow
        }
    }

    /// <summary>
    /// Generates a unique transaction number for pick completions.
    /// </summary>
    private async Task<string> GenerateTransactionNumber(CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var prefix = $"TXN-PICK-{date:yyyyMMdd}";
        
        // Get count of transactions
        var count = await transactionRepository.CountAsync(cancellationToken);
        
        return $"{prefix}-{(count + 1):D6}";
    }
}

