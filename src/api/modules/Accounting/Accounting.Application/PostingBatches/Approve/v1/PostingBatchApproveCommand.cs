namespace Accounting.Application.PostingBatches.Approve.v1;

public sealed record PostingBatchApproveCommand(
    DefaultIdType Id,
    string ApprovedBy
) : IRequest<DefaultIdType>;
