namespace Accounting.Application.PostingBatches.Search.v1;

public sealed class PostingBatchSearchQuery : PaginationFilter, IRequest<PagedList<PostingBatchSearchResponse>>
{
    public string? BatchNumber { get; init; }
    public DefaultIdType? PeriodId { get; init; }
    public string? Status { get; init; }
    public string? ApprovalStatus { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
}

