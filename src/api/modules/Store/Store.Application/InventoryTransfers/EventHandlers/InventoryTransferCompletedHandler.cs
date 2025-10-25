using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.EventHandlers;

/// <summary>
/// Event handler that creates an inventory transaction when a transfer is completed.
/// </summary>
/// <remarks>
/// This handler creates inventory transactions for the completed transfer:
/// - OUT transaction at source warehouse
/// - IN transaction at destination warehouse
/// This provides complete audit trail for inventory movements between warehouses.
/// </remarks>
public sealed class InventoryTransferCompletedHandler(
    ILogger<InventoryTransferCompletedHandler> logger,
    [FromKeyedServices("store:inventory-transactions")]
    IRepository<InventoryTransaction> transactionRepository)
    : INotificationHandler<InventoryTransferCompleted>
{
    /// <summary>
    /// Handles the InventoryTransferCompleted event by creating inventory transactions.
    /// </summary>
    /// <param name="notification">The domain event containing the completed transfer.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(InventoryTransferCompleted notification, CancellationToken cancellationToken)
    {
        var transfer = notification.InventoryTransfer;

        logger.LogInformation(
            "Processing inventory transfer completion: {TransferNumber} from Warehouse {FromWarehouseId} to {ToWarehouseId}",
            transfer.TransferNumber,
            transfer.FromWarehouseId,
            transfer.ToWarehouseId);

        try
        {
            // Create transactions for each item in the transfer
            foreach (var item in transfer.Items)
            {
                // OUT transaction at source warehouse
                var outTransactionNumber = await GenerateTransactionNumber("OUT", cancellationToken);
                var outTransaction = InventoryTransaction.Create(
                    transactionNumber: outTransactionNumber,
                    itemId: item.ItemId,
                    warehouseId: transfer.FromWarehouseId,
                    warehouseLocationId: transfer.FromLocationId,
                    purchaseOrderId: null,
                    transactionType: "TRANSFER",
                    reason: "TRANSFER_OUT",
                    quantity: item.Quantity,
                    quantityBefore: 0, // Would need current stock level
                    unitCost: item.UnitPrice,
                    transactionDate: DateTime.UtcNow,
                    reference: transfer.TransferNumber,
                    notes: $"Transfer {transfer.TransferNumber} to Warehouse {transfer.ToWarehouseId}. Reason: {transfer.Reason}",
                    performedBy: null,
                    isApproved: true);

                await transactionRepository.AddAsync(outTransaction, cancellationToken);

                // IN transaction at destination warehouse
                var inTransactionNumber = await GenerateTransactionNumber("IN", cancellationToken);
                var inTransaction = InventoryTransaction.Create(
                    transactionNumber: inTransactionNumber,
                    itemId: item.ItemId,
                    warehouseId: transfer.ToWarehouseId,
                    warehouseLocationId: transfer.ToLocationId,
                    purchaseOrderId: null,
                    transactionType: "TRANSFER",
                    reason: "TRANSFER_IN",
                    quantity: item.Quantity,
                    quantityBefore: 0, // Would need current stock level
                    unitCost: item.UnitPrice,
                    transactionDate: DateTime.UtcNow,
                    reference: transfer.TransferNumber,
                    notes: $"Transfer {transfer.TransferNumber} from Warehouse {transfer.FromWarehouseId}. Reason: {transfer.Reason}",
                    performedBy: null,
                    isApproved: true);

                await transactionRepository.AddAsync(inTransaction, cancellationToken);
            }

            await transactionRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Created inventory transactions for transfer {TransferNumber}: {ItemCount} items, {TotalTransactions} transactions",
                transfer.TransferNumber,
                transfer.Items.Count,
                transfer.Items.Count * 2);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to create inventory transactions for transfer completion: {TransferNumber}",
                transfer.TransferNumber);
            // Don't rethrow - event handlers should not break the main flow
        }
    }

    /// <summary>
    /// Generates a unique transaction number for transfer transactions.
    /// </summary>
    private async Task<string> GenerateTransactionNumber(string type, CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var prefix = $"TXN-TRF{type}-{date:yyyyMMdd}";
        
        // Get count of transactions with this prefix
        var count = await transactionRepository.CountAsync(cancellationToken);
        
        return $"{prefix}-{(count + 1):D6}";
    }
}

