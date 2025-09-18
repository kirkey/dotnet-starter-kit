using Accounting.Application.PostingBatches.Dtos;

namespace Accounting.Application.PostingBatches.Queries;

public class GetPostingBatchByIdQuery(DefaultIdType id) : IRequest<PostingBatchDto>
{
    public DefaultIdType Id { get; set; } = id;
}