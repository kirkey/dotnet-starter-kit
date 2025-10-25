using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.EventHandlers;

/// <summary>
/// Event handler that creates an inventory transaction when a transfer is created.
/// </summary>
/// <remarks>
/// This handler maintains an audit trail of transfer creation by creating
/// ADJUSTMENT type transactions with reason "TRANSFER_CREATED". This provides
/// visibility into when transfers are initiated.
/// </remarks>
public sealed class InventoryTransferCreatedHandler(
    ILogger<InventoryTransferCreatedHandler> logger,
    [FromKeyedServices("store:inventory-transactions")]
    IRepository<InventoryTransaction> transactionRepository)
    : INotificationHandler<InventoryTransferCreated>
{
    /// <summary>
    /// Handles the InventoryTransferCreated event by creating inventory transaction records.
    /// </summary>
    /// <param name="notification">The domain event containing the created transfer.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(InventoryTransferCreated notification, CancellationToken cancellationToken)
    {
        var transfer = notification.InventoryTransfer;

        logger.LogInformation(
            "Processing inventory transfer creation: {TransferNumber} from Warehouse {FromWarehouseId} to {ToWarehouseId}",
            transfer.TransferNumber,
            transfer.FromWarehouseId,
            transfer.ToWarehouseId);

        try
        {
            // Create adjustment transactions for each item to record the transfer initiation
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
                    reason: "TRANSFER_CREATED",
                    quantity: item.Quantity,
                    quantityBefore: 0,
                    unitCost: item.UnitPrice,
                    transactionDate: DateTime.UtcNow,
                    reference: transfer.TransferNumber,
                    notes: $"Transfer {transfer.TransferNumber} created to Warehouse {transfer.ToWarehouseId}. Status: {transfer.Status}. Reason: {transfer.Reason}",
                    performedBy: transfer.RequestedBy,
                    isApproved: false); // Pending approval

                await transactionRepository.AddAsync(transaction, cancellationToken);
            }

            await transactionRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Created inventory transactions for transfer creation {TransferNumber}: {ItemCount} items",
                transfer.TransferNumber,
                transfer.Items.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to create inventory transactions for transfer creation: {TransferNumber}",
                transfer.TransferNumber);
            // Don't rethrow - event handlers should not break the main flow
        }
    }

    /// <summary>
    /// Generates a unique transaction number for transfer created transactions.
    /// </summary>
    private async Task<string> GenerateTransactionNumber(CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var prefix = $"TXN-TRFCR-{date:yyyyMMdd}";
        
        // Get count of transactions
        var count = await transactionRepository.CountAsync(cancellationToken);
        
        return $"{prefix}-{(count + 1):D6}";
    }
}

