namespace Accounting.Application.WriteOffs.Approve.v1;

/// <summary>
/// Command to approve a write-off.
/// The approver is automatically determined from the current user session.
/// </summary>
public sealed record ApproveWriteOffCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

