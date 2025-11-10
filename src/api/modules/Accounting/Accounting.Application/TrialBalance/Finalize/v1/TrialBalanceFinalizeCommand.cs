namespace Accounting.Application.TrialBalance.Finalize.v1;

/// <summary>
/// Command to finalize a trial balance report.
/// The finalizer is automatically determined from the current user session.
/// </summary>
/// <remarks>
/// Finalize Rules:
/// - Trial balance must be balanced (TotalDebits = TotalCredits)
/// - Accounting equation must balance (Assets = Liabilities + Equity)
/// - Sets Status to "Finalized"
/// - Records who finalized and when (from session)
/// - Cannot be modified after finalization (must reopen first)
/// </remarks>
public sealed record TrialBalanceFinalizeCommand(
    DefaultIdType Id
) : IRequest<DefaultIdType>;
