using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.EventHandlers;

/// <summary>
/// Event handler that creates an inventory transaction when a transfer is approved.
/// </summary>
/// <remarks>
/// This handler records the approval event in the audit trail with ADJUSTMENT
/// transactions. This marks the point where the transfer is authorized to proceed.
/// </remarks>
public sealed class InventoryTransferApprovedHandler(
    ILogger<InventoryTransferApprovedHandler> logger,
    [FromKeyedServices("store:inventory-transactions")]
    IRepository<InventoryTransaction> transactionRepository)
    : INotificationHandler<InventoryTransferApproved>
{
    /// <summary>
    /// Handles the InventoryTransferApproved event by creating inventory transaction records.
    /// </summary>
    /// <param name="notification">The domain event containing the approved transfer.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(InventoryTransferApproved notification, CancellationToken cancellationToken)
    {
        var transfer = notification.InventoryTransfer;

        logger.LogInformation(
            "Processing inventory transfer approval: {TransferNumber} approved by {ApprovedBy}",
            transfer.TransferNumber,
            transfer.ApprovedBy);

        try
        {
            // Create adjustment transactions for each item to record the approval
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
                    reason: "TRANSFER_APPROVED",
                    quantity: item.Quantity,
                    quantityBefore: 0,
                    unitCost: item.UnitPrice,
                    transactionDate: DateTime.UtcNow,
                    reference: transfer.TransferNumber,
                    notes: $"Transfer {transfer.TransferNumber} approved by {transfer.ApprovedBy}. Ready for shipment.",
                    performedBy: transfer.ApprovedBy,
                    isApproved: true);

                await transactionRepository.AddAsync(transaction, cancellationToken);
            }

            await transactionRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Created inventory transactions for transfer approval {TransferNumber}: {ItemCount} items",
                transfer.TransferNumber,
                transfer.Items.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to create inventory transactions for transfer approval: {TransferNumber}",
                transfer.TransferNumber);
            // Don't rethrow - event handlers should not break the main flow
        }
    }

    /// <summary>
    /// Generates a unique transaction number for transfer approved transactions.
    /// </summary>
    private async Task<string> GenerateTransactionNumber(CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var prefix = $"TXN-TRFAP-{date:yyyyMMdd}";
        
        // Get count of transactions
        var count = await transactionRepository.CountAsync(cancellationToken);
        
        return $"{prefix}-{(count + 1):D6}";
    }
}

