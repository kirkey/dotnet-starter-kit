namespace Accounting.Application.Accruals.Approve;

/// <summary>
/// Command to approve an accrual.
/// The approver is automatically determined from the current user session.
/// </summary>
public sealed record ApproveAccrualCommand(
    DefaultIdType AccrualId
) : IRequest<DefaultIdType>;
