using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.EventHandlers;

/// <summary>
/// Event handler that updates inventory transaction approval status when a stock adjustment is approved.
/// </summary>
/// <remarks>
/// This handler finds the transaction created for this adjustment and approves it,
/// maintaining consistency between the adjustment approval and transaction approval.
/// This ensures the audit trail accurately reflects authorized adjustments.
/// </remarks>
public sealed class StockAdjustmentApprovedHandler(
    ILogger<StockAdjustmentApprovedHandler> logger,
    [FromKeyedServices("store:inventory-transactions")]
    IRepository<InventoryTransaction> transactionRepository,
    [FromKeyedServices("store:inventory-transactions")]
    IReadRepository<InventoryTransaction> readTransactionRepository)
    : INotificationHandler<StockAdjustmentApproved>
{
    /// <summary>
    /// Handles the StockAdjustmentApproved event by approving the related inventory transaction.
    /// </summary>
    /// <param name="notification">The domain event containing the approved stock adjustment.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(StockAdjustmentApproved notification, CancellationToken cancellationToken)
    {
        var adjustment = notification.StockAdjustment;

        logger.LogInformation(
            "Processing stock adjustment approval: {AdjustmentNumber} approved by {ApprovedBy}",
            adjustment.AdjustmentNumber,
            adjustment.ApprovedBy);

        try
        {
            // Find the transaction created for this adjustment (by reference number)
            var spec = new FSH.Starter.WebApi.Store.Application.InventoryTransactions.Specs.InventoryTransactionByNumberSpec(adjustment.AdjustmentNumber);
            var transactions = await readTransactionRepository.ListAsync(spec, cancellationToken);
            
            if (transactions.Count > 0)
            {
                foreach (var transaction in transactions)
                {
                    // Approve the transaction
                    transaction.Approve(adjustment.ApprovedBy ?? "System");
                    await transactionRepository.UpdateAsync(transaction, cancellationToken);
                }
                
                await transactionRepository.SaveChangesAsync(cancellationToken);

                logger.LogInformation(
                    "Approved {Count} inventory transaction(s) for stock adjustment {AdjustmentNumber}",
                    transactions.Count,
                    adjustment.AdjustmentNumber);
            }
            else
            {
                logger.LogWarning(
                    "No inventory transactions found for stock adjustment {AdjustmentNumber}",
                    adjustment.AdjustmentNumber);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to approve inventory transactions for stock adjustment: {AdjustmentNumber}",
                adjustment.AdjustmentNumber);
            // Don't rethrow - event handlers should not break the main flow
        }
    }
}

