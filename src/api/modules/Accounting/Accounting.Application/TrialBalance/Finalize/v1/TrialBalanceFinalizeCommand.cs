namespace Accounting.Application.TrialBalance.Finalize.v1;

/// <summary>
/// Command to finalize a trial balance report.
/// </summary>
/// <remarks>
/// Finalize Rules:
/// - Trial balance must be balanced (TotalDebits = TotalCredits)
/// - Accounting equation must balance (Assets = Liabilities + Equity)
/// - Sets Status to "Finalized"
/// - Records who finalized and when
/// - Cannot be modified after finalization (must reopen first)
/// </remarks>
public sealed record TrialBalanceFinalizeCommand(
    DefaultIdType Id,
    string FinalizedBy
) : IRequest<DefaultIdType>;
