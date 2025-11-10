namespace Accounting.Application.PostingBatches.Search.v1;

/// <summary>
/// Specification for searching posting batches with filtering and pagination.
/// Projects results to <see cref="PostingBatchSearchResponse"/>.
/// </summary>
public sealed class PostingBatchSearchSpec : EntitiesByPaginationFilterSpec<PostingBatch, PostingBatchSearchResponse>
{
    public PostingBatchSearchSpec(PostingBatchSearchQuery query) : base(query)
    {
        Query
            .Include(b => b.JournalEntries)
            .OrderByDescending(b => b.BatchDate, !query.HasOrderBy())
            .ThenBy(b => b.BatchNumber)
            .Where(b => b.BatchNumber.Contains(query.BatchNumber!), !string.IsNullOrWhiteSpace(query.BatchNumber))
            .Where(b => b.PeriodId == query.PeriodId!.Value, query.PeriodId.HasValue && query.PeriodId.Value != DefaultIdType.Empty)
            .Where(b => b.Status == query.Status!, !string.IsNullOrWhiteSpace(query.Status))
            .Where(b => b.Status == query.ApprovalStatus!, !string.IsNullOrWhiteSpace(query.ApprovalStatus))
            .Where(b => b.BatchDate >= query.StartDate!.Value, query.StartDate.HasValue)
            .Where(b => b.BatchDate <= query.EndDate!.Value, query.EndDate.HasValue);
    }
}

