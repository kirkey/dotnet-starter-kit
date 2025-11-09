namespace Accounting.Application.PostingBatches.Approve.v1;

/// <summary>
/// Command to approve a posting batch.
/// The approver is automatically determined from the current user session.
/// </summary>
public sealed record PostingBatchApproveCommand(
    DefaultIdType Id
) : IRequest<DefaultIdType>;
