namespace Accounting.Application.BankReconciliations.Reject.v1;

/// <summary>
/// Command to reject a completed bank reconciliation for rework.
/// Transitions reconciliation back to pending status and records rejection reason in notes.
/// </summary>
public sealed class RejectBankReconciliationCommand : BaseRequest, IRequest
{
    /// <summary>
    /// User identifier or name who is rejecting the reconciliation.
    /// Required. Maximum 256 characters.
    /// Used for audit trail and compliance reporting.
    /// </summary>
    public string RejectedBy { get; set; } = string.Empty;

    /// <summary>
    /// Optional reason for rejection.
    /// Maximum 2048 characters.
    /// Appended to the reconciliation notes for reference.
    /// </summary>
    public string? Reason { get; set; }
}
