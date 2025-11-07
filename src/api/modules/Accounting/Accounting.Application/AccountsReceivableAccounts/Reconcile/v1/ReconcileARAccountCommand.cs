namespace Accounting.Application.AccountsReceivableAccounts.Reconcile.v1;

public sealed record ReconcileARAccountCommand(DefaultIdType Id, decimal SubsidiaryLedgerBalance) : IRequest<DefaultIdType>;

