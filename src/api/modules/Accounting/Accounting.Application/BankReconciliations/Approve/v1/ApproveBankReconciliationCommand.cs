namespace Accounting.Application.BankReconciliations.Approve.v1;

/// <summary>
/// Command to approve a completed bank reconciliation.
/// Transitions reconciliation to approved status and sets IsReconciled flag to true.
/// </summary>
public sealed class ApproveBankReconciliationCommand : BaseRequest, IRequest
{
    /// <summary>
    /// User identifier or name who is approving the reconciliation.
    /// Required. Maximum 256 characters.
    /// Used for audit trail and compliance reporting.
    /// </summary>
    public string ApprovedBy { get; set; } = string.Empty;
}
