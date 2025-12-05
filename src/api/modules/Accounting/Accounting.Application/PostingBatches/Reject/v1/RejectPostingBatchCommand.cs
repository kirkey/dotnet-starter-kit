namespace Accounting.Application.PostingBatches.Reject.v1;

/// <summary>
/// Command to reject a posting batch.
/// The rejector is automatically determined from the current user session.
/// </summary>
public sealed record RejectPostingBatchCommand(
    DefaultIdType Id,
    string? Reason = null
) : IRequest<DefaultIdType>;

