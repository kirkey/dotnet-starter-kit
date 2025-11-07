namespace Accounting.Application.PostingBatches.Reject.v1;

public sealed record PostingBatchRejectCommand(DefaultIdType Id, string RejectedBy) : IRequest<DefaultIdType>;

