namespace Accounting.Application.PostingBatches.Reverse.v1;

public sealed record ReversePostingBatchCommand(DefaultIdType Id, string Reason) : IRequest<DefaultIdType>;

