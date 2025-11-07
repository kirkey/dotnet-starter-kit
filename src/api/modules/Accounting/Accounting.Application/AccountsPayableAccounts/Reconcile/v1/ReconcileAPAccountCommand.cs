namespace Accounting.Application.AccountsPayableAccounts.Reconcile.v1;

public sealed record ReconcileAPAccountCommand(DefaultIdType Id, decimal SubsidiaryLedgerBalance) : IRequest<DefaultIdType>;

