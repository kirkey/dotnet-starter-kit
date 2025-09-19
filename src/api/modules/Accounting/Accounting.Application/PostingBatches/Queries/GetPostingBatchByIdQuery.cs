using Accounting.Application.PostingBatches.Responses;

namespace Accounting.Application.PostingBatches.Queries;

public class GetPostingBatchByIdQuery(DefaultIdType id) : IRequest<PostingBatchResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
