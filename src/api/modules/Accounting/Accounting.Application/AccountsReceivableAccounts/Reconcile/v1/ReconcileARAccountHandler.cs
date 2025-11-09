namespace Accounting.Application.AccountsReceivableAccounts.Reconcile.v1;

public sealed class ReconcileArAccountHandler(
    IRepository<AccountsReceivableAccount> repository,
    ILogger<ReconcileArAccountHandler> logger)
    : IRequestHandler<ReconcileArAccountCommand, DefaultIdType>
{
    private readonly IRepository<AccountsReceivableAccount> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<ReconcileArAccountHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(ReconcileArAccountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Reconciling AR account {Id} with subsidiary ledger balance {Balance}", 
            request.Id, request.SubsidiaryLedgerBalance);

        var account = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (account == null) throw new NotFoundException($"AR account with ID {request.Id} not found");

        account.Reconcile(request.SubsidiaryLedgerBalance);
        await _repository.UpdateAsync(account, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("AR account {AccountNumber} reconciled: Variance={Variance}, IsReconciled={IsReconciled}", 
            account.AccountNumber, account.ReconciliationVariance, account.IsReconciled);
        return account.Id;
    }
}
