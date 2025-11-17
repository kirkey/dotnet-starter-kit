namespace Accounting.Application.AccountReconciliations.Create.v1;

/// <summary>
/// Command to create a new account reconciliation.
/// </summary>
public sealed record CreateAccountReconciliationCommand(
    DefaultIdType GeneralLedgerAccountId,
    DefaultIdType AccountingPeriodId,
    decimal GlBalance,
    decimal SubsidiaryLedgerBalance,
    string SubsidiaryLedgerSource,
    DateTime ReconciliationDate,
    string? VarianceExplanation = null
) : IRequest<DefaultIdType>;

