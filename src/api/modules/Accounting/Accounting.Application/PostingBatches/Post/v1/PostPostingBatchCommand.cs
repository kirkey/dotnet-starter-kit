namespace Accounting.Application.PostingBatches.Post.v1;

public sealed record PostPostingBatchCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

