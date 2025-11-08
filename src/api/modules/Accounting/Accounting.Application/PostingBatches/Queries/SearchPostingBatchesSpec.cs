using Accounting.Application.PostingBatches.Responses;

namespace Accounting.Application.PostingBatches.Queries;

/// <summary>
/// Specification for searching posting batches with filtering and pagination.
/// </summary>
public class SearchPostingBatchesSpec : EntitiesByPaginationFilterSpec<PostingBatch, PostingBatchResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchPostingBatchesSpec"/> class.
    /// </summary>
    /// <param name="request">The search posting batches query containing filter criteria and pagination parameters.</param>
    public SearchPostingBatchesSpec(SearchPostingBatchesQuery request)
        : base(request)
    {
        Query
            .Where(x => x.BatchNumber.Contains(request.BatchNumber!), !string.IsNullOrWhiteSpace(request.BatchNumber))
            .Where(x => x.Status == request.Status, !string.IsNullOrWhiteSpace(request.Status))
            .Where(x => x.Status == request.ApprovalStatus, !string.IsNullOrWhiteSpace(request.ApprovalStatus))
            .OrderByDescending(x => x.BatchDate, !request.HasOrderBy())
            .ThenBy(x => x.BatchNumber);
    }
}

