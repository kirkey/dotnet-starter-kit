namespace Accounting.Application.AccountsReceivableAccounts.Reconcile.v1;

public sealed record ReconcileArAccountCommand(DefaultIdType Id, decimal SubsidiaryLedgerBalance) : IRequest<DefaultIdType>;

