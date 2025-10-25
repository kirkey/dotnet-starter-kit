using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.PickLists.EventHandlers;

/// <summary>
/// Event handler that creates inventory transactions when a pick list is created.
/// </summary>
/// <remarks>
/// This handler maintains an audit trail of pick list creation by creating
/// ADJUSTMENT type transactions with reason "PICK_LIST_CREATED". This provides
/// visibility into when pick lists are initiated for order fulfillment.
/// </remarks>
public sealed class PickListCreatedHandler(
    ILogger<PickListCreatedHandler> logger,
    [FromKeyedServices("store:inventory-transactions")] IRepository<InventoryTransaction> transactionRepository) : INotificationHandler<PickListCreated>
{
    /// <summary>
    /// Handles the PickListCreated event by creating inventory transaction records.
    /// </summary>
    /// <param name="notification">The domain event containing the created pick list.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(PickListCreated notification, CancellationToken cancellationToken)
    {
        var pickList = notification.PickList;

        logger.LogInformation(
            "Processing pick list creation: {PickListNumber} at Warehouse {WarehouseId}, Type: {PickingType}",
            pickList.PickListNumber,
            pickList.WarehouseId,
            pickList.PickingType);

        try
        {
            // Create adjustment transactions for each item to record pick list creation
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
                    reason: "PICK_LIST_CREATED",
                    quantity: item.QuantityToPick,
                    quantityBefore: 0,
                    unitCost: 0m,
                    transactionDate: DateTime.UtcNow,
                    reference: pickList.PickListNumber,
                    notes: $"Pick List {pickList.PickListNumber} created. Type: {pickList.PickingType}, Priority: {pickList.Priority}. Reference: {pickList.ReferenceNumber ?? "N/A"}",
                    performedBy: null,
                    isApproved: false); // Pending completion

                await transactionRepository.AddAsync(transaction, cancellationToken);
            }

            await transactionRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Created {Count} inventory transactions for pick list creation {PickListNumber}",
                pickList.Items.Count,
                pickList.PickListNumber);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to create inventory transactions for pick list creation: {PickListNumber}",
                pickList.PickListNumber);
            // Don't rethrow - event handlers should not break the main flow
        }
    }

    /// <summary>
    /// Generates a unique transaction number for pick list created transactions.
    /// </summary>
    private async Task<string> GenerateTransactionNumber(CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var prefix = $"TXN-PICKCR-{date:yyyyMMdd}";
        
        // Get count of transactions
        var count = await transactionRepository.CountAsync(cancellationToken);
        
        return $"{prefix}-{(count + 1):D6}";
    }
}

