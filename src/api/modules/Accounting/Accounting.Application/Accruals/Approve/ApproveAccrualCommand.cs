namespace Accounting.Application.Accruals.Approve;

/// <summary>
/// Command to approve an accrual.
/// </summary>
public sealed record ApproveAccrualCommand(
    DefaultIdType AccrualId,
    string ApprovedBy
) : IRequest<DefaultIdType>;
