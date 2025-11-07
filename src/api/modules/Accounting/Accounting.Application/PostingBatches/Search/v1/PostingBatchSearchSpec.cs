namespace Accounting.Application.PostingBatches.Search.v1;

public sealed class PostingBatchSearchSpec : Specification<PostingBatch>
{
    public PostingBatchSearchSpec(PostingBatchSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (!string.IsNullOrWhiteSpace(query.BatchNumber))
        {
            Query.Where(b => b.BatchNumber.Contains(query.BatchNumber));
        }

        if (query.PeriodId.HasValue && query.PeriodId.Value != DefaultIdType.Empty)
        {
            Query.Where(b => b.PeriodId == query.PeriodId.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            Query.Where(b => b.Status == query.Status);
        }

        if (!string.IsNullOrWhiteSpace(query.ApprovalStatus))
        {
            Query.Where(b => b.ApprovalStatus == query.ApprovalStatus);
        }

        if (query.StartDate.HasValue)
        {
            Query.Where(b => b.BatchDate >= query.StartDate.Value);
        }

        if (query.EndDate.HasValue)
        {
            Query.Where(b => b.BatchDate <= query.EndDate.Value);
        }

        Query.Skip(query.PageNumber * query.PageSize)
             .Take(query.PageSize);

        Query.OrderByDescending(b => b.BatchDate)
             .ThenBy(b => b.BatchNumber);
    }
}

