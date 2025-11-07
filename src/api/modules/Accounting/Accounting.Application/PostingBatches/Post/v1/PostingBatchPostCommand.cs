namespace Accounting.Application.PostingBatches.Post.v1;

public sealed record PostingBatchPostCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

