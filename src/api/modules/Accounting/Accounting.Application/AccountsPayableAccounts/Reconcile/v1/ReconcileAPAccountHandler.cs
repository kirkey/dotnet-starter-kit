namespace Accounting.Application.AccountsPayableAccounts.Reconcile.v1;

public sealed class ReconcileAPAccountHandler(
    IRepository<AccountsPayableAccount> repository,
    ILogger<ReconcileAPAccountHandler> logger)
    : IRequestHandler<ReconcileAPAccountCommand, DefaultIdType>
{
    private readonly IRepository<AccountsPayableAccount> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<ReconcileAPAccountHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(ReconcileAPAccountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Reconciling AP account {Id} with subsidiary ledger balance {Balance}", 
            request.Id, request.SubsidiaryLedgerBalance);

        var account = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (account == null) throw new NotFoundException($"AP account with ID {request.Id} not found");

        account.Reconcile(request.SubsidiaryLedgerBalance);
        await _repository.UpdateAsync(account, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("AP account {AccountNumber} reconciled: Variance={Variance}, IsReconciled={IsReconciled}", 
            account.AccountNumber, account.ReconciliationVariance, account.IsReconciled);
        return account.Id;
    }
}
