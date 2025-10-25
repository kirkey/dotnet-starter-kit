using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.EventHandlers;

/// <summary>
/// Event handler that creates an inventory transaction when a transfer is marked in transit.
/// </summary>
/// <remarks>
/// This handler records when the transfer shipment begins moving from source to destination.
/// Creates ADJUSTMENT transactions to track the in-transit status in the audit trail.
/// </remarks>
public sealed class InventoryTransferInTransitHandler(
    ILogger<InventoryTransferInTransitHandler> logger,
    [FromKeyedServices("store:inventory-transactions")]
    IRepository<InventoryTransaction> transactionRepository)
    : INotificationHandler<InventoryTransferInTransit>
{
    /// <summary>
    /// Handles the InventoryTransferInTransit event by creating inventory transaction records.
    /// </summary>
    /// <param name="notification">The domain event containing the in-transit transfer.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(InventoryTransferInTransit notification, CancellationToken cancellationToken)
    {
        var transfer = notification.InventoryTransfer;

        logger.LogInformation(
            "Processing inventory transfer in-transit status: {TransferNumber}",
            transfer.TransferNumber);

        try
        {
            // Create adjustment transactions for each item to record in-transit status
            foreach (var item in transfer.Items)
            {
                var transactionNumber = await GenerateTransactionNumber(cancellationToken);

                var transaction = InventoryTransaction.Create(
                    transactionNumber: transactionNumber,
                    itemId: item.ItemId,
                    warehouseId: transfer.FromWarehouseId,
                    warehouseLocationId: transfer.FromLocationId,
                    purchaseOrderId: null,
                    transactionType: "ADJUSTMENT",
                    reason: "TRANSFER_IN_TRANSIT",
                    quantity: item.Quantity,
                    quantityBefore: 0,
                    unitCost: item.UnitPrice,
                    transactionDate: DateTime.UtcNow,
                    reference: transfer.TransferNumber,
                    notes: $"Transfer {transfer.TransferNumber} in transit to Warehouse {transfer.ToWarehouseId}. Tracking: {transfer.TrackingNumber ?? "N/A"}",
                    performedBy: null,
                    isApproved: true);

                await transactionRepository.AddAsync(transaction, cancellationToken);
            }

            await transactionRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Created inventory transactions for transfer in-transit {TransferNumber}: {ItemCount} items",
                transfer.TransferNumber,
                transfer.Items.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to create inventory transactions for transfer in-transit: {TransferNumber}",
                transfer.TransferNumber);
            // Don't rethrow - event handlers should not break the main flow
        }
    }

    /// <summary>
    /// Generates a unique transaction number for transfer in-transit transactions.
    /// </summary>
    private async Task<string> GenerateTransactionNumber(CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var prefix = $"TXN-TRFIT-{date:yyyyMMdd}";
        
        // Get count of transactions
        var count = await transactionRepository.CountAsync(cancellationToken);
        
        return $"{prefix}-{(count + 1):D6}";
    }
}

