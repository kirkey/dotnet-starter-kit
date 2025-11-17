namespace Accounting.Application.AccountReconciliations.Update.v1;

/// <summary>
/// Handler for updating account reconciliation.
/// </summary>
public sealed class UpdateAccountReconciliationHandler(
    [FromKeyedServices("accounting:account-reconciliations")] IRepository<AccountReconciliation> repository,
    ILogger<UpdateAccountReconciliationHandler> logger)
    : IRequestHandler<UpdateAccountReconciliationCommand>
{
    public async Task Handle(UpdateAccountReconciliationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var reconciliation = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (reconciliation == null)
            throw new AccountReconciliationNotFoundException(request.Id);

        if (reconciliation.ReconciliationStatus == "Approved")
            throw new CannotUpdateApprovedReconciliationException(request.Id);

        // Update balances if provided
        if (request.GlBalance.HasValue || request.SubsidiaryLedgerBalance.HasValue)
        {
            var glBalance = request.GlBalance ?? reconciliation.GlBalance;
            var subsidiaryBalance = request.SubsidiaryLedgerBalance ?? reconciliation.SubsidiaryLedgerBalance;
            reconciliation.UpdateBalances(glBalance, subsidiaryBalance, request.VarianceExplanation);
        }

        // Update line item count if provided
        if (request.LineItemCount.HasValue)
            reconciliation.SetLineItemCount(request.LineItemCount.Value);

        // Record adjusting entries if flag is set
        if (request.AdjustingEntriesRecorded.HasValue && request.AdjustingEntriesRecorded.Value)
            reconciliation.RecordAdjustingEntries();

        await repository.UpdateAsync(reconciliation, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Account reconciliation {Id} updated with variance {Variance:C}",
            request.Id, reconciliation.Variance);
    }
}

