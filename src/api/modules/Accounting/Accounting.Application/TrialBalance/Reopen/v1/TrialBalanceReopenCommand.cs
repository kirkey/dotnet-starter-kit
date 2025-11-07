namespace Accounting.Application.TrialBalance.Reopen.v1;

/// <summary>
/// Command to reopen a finalized trial balance report.
/// </summary>
/// <remarks>
/// Reopen Rules:
/// - Can only reopen finalized trial balances
/// - Requires a reason for audit trail
/// - Sets Status back to "Draft"
/// - Allows modifications after reopening
/// - Reason is appended to Notes
/// </remarks>
public sealed record TrialBalanceReopenCommand(
    DefaultIdType Id,
    string Reason
) : IRequest<DefaultIdType>;

