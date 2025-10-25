using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.EventHandlers;

/// <summary>
/// Event handler that creates an inventory transaction when a transfer is cancelled.
/// </summary>
/// <remarks>
/// This handler records the cancellation event in the audit trail with ADJUSTMENT
/// transactions. This provides visibility into cancelled transfers and their reasons.
/// </remarks>
public sealed class InventoryTransferCancelledHandler(
    ILogger<InventoryTransferCancelledHandler> logger,
    [FromKeyedServices("store:inventory-transactions")]
    IRepository<InventoryTransaction> transactionRepository)
    : INotificationHandler<InventoryTransferCancelled>
{
    /// <summary>
    /// Handles the InventoryTransferCancelled event by creating inventory transaction records.
    /// </summary>
    /// <param name="notification">The domain event containing the cancelled transfer.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(InventoryTransferCancelled notification, CancellationToken cancellationToken)
    {
        var transfer = notification.InventoryTransfer;

        logger.LogInformation(
            "Processing inventory transfer cancellation: {TransferNumber}, Reason: {Reason}",
            transfer.TransferNumber,
            notification.Reason);

        try
        {
            // Create adjustment transactions for each item to record the cancellation
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
                    reason: "TRANSFER_CANCELLED",
                    quantity: item.Quantity,
                    quantityBefore: 0,
                    unitCost: item.UnitPrice,
                    transactionDate: DateTime.UtcNow,
                    reference: transfer.TransferNumber,
                    notes: $"Transfer {transfer.TransferNumber} cancelled. Reason: {notification.Reason}",
                    performedBy: null,
                    isApproved: true);

                await transactionRepository.AddAsync(transaction, cancellationToken);
            }

            await transactionRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Created inventory transactions for transfer cancellation {TransferNumber}: {ItemCount} items",
                transfer.TransferNumber,
                transfer.Items.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to create inventory transactions for transfer cancellation: {TransferNumber}",
                transfer.TransferNumber);
            // Don't rethrow - event handlers should not break the main flow
        }
    }

    /// <summary>
    /// Generates a unique transaction number for transfer cancelled transactions.
    /// </summary>
    private async Task<string> GenerateTransactionNumber(CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var prefix = $"TXN-TRFCN-{date:yyyyMMdd}";
        
        // Get count of transactions
        var count = await transactionRepository.CountAsync(cancellationToken);
        
        return $"{prefix}-{(count + 1):D6}";
    }
}

