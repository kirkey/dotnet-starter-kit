namespace Accounting.Application.BankReconciliations.Complete.v1;

/// <summary>
/// Command to complete a bank reconciliation.
/// Marks reconciliation as completed and validates balance matching before transitioning to approval workflow.
/// </summary>
public sealed class CompleteBankReconciliationCommand : BaseRequest, IRequest
{
    /// <summary>
    /// User identifier or name who is completing the reconciliation.
    /// Required. Maximum 256 characters.
    /// Used for audit trail and approval workflow tracking.
    /// </summary>
    public string ReconciledBy { get; set; } = string.Empty;
}
