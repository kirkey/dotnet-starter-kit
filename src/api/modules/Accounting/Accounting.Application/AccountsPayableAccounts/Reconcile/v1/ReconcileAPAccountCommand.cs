namespace Accounting.Application.AccountsPayableAccounts.Reconcile.v1;

public sealed record ReconcileApAccountCommand(DefaultIdType Id, decimal SubsidiaryLedgerBalance) : IRequest<DefaultIdType>;

