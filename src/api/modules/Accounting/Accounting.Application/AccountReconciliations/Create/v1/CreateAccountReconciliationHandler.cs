namespace Accounting.Application.AccountReconciliations.Create.v1;

/// <summary>
/// Handler for creating an account reconciliation.
/// </summary>
public sealed class CreateAccountReconciliationHandler(
    [FromKeyedServices("accounting:account-reconciliations")] IRepository<AccountReconciliation> repository,
    ILogger<CreateAccountReconciliationHandler> logger)
    : IRequestHandler<CreateAccountReconciliationCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateAccountReconciliationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        logger.LogInformation("Creating account reconciliation for GL Account {GlAccountId} in period {PeriodId}",
            request.GeneralLedgerAccountId, request.AccountingPeriodId);

        var reconciliation = AccountReconciliation.Create(
            request.GeneralLedgerAccountId,
            request.AccountingPeriodId,
            request.GlBalance,
            request.SubsidiaryLedgerBalance,
            request.SubsidiaryLedgerSource,
            request.ReconciliationDate,
            request.VarianceExplanation);

        await repository.AddAsync(reconciliation, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Account reconciliation created: {Id} with variance {Variance:C}",
            reconciliation.Id, reconciliation.Variance);
        return reconciliation.Id;
    }
}

