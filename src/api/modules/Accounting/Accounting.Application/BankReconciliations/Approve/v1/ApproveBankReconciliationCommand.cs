namespace Accounting.Application.BankReconciliations.Approve.v1;

/// <summary>
/// Command to approve a completed bank reconciliation.
/// Transitions reconciliation to approved status and sets IsReconciled flag to true.
/// The approver is automatically determined from the current user session.
/// </summary>
public sealed class ApproveBankReconciliationCommand : BaseRequest, IRequest
{
    // Approver information is obtained from ICurrentUser in the handler
}
