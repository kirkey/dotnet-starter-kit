namespace Accounting.Application.PostingBatches.Reverse.v1;

public sealed record PostingBatchReverseCommand(DefaultIdType Id, string Reason) : IRequest<DefaultIdType>;

