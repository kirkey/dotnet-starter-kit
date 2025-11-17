namespace Accounting.Application.AccountReconciliations.Update.v1;

/// <summary>
/// Command to update account reconciliation balances.
/// </summary>
public sealed record UpdateAccountReconciliationCommand(
    DefaultIdType Id,
    decimal? GlBalance = null,
    decimal? SubsidiaryLedgerBalance = null,
    string? VarianceExplanation = null,
    int? LineItemCount = null,
    bool? AdjustingEntriesRecorded = null
) : IRequest;

