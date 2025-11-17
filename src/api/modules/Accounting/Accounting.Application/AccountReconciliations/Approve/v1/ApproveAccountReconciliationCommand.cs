namespace Accounting.Application.AccountReconciliations.Approve.v1;

/// <summary>
/// Command to approve an account reconciliation.
/// </summary>
public sealed record ApproveAccountReconciliationCommand(
    DefaultIdType Id,
    DefaultIdType ApproverId,
    string? ApproverName = null,
    string? Remarks = null
) : IRequest;

