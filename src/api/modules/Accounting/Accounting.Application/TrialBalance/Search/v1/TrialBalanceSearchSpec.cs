namespace Accounting.Application.TrialBalance.Search.v1;

/// <summary>
/// Specification for searching trial balance reports.
/// </summary>
public sealed class TrialBalanceSearchSpec : Specification<Domain.Entities.TrialBalance>
{
    public TrialBalanceSearchSpec(TrialBalanceSearchQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (query.PeriodId.HasValue && query.PeriodId.Value != DefaultIdType.Empty)
        {
            Query.Where(tb => tb.PeriodId == query.PeriodId.Value);
        }

        if (query.StartDate.HasValue)
        {
            Query.Where(tb => tb.PeriodStartDate >= query.StartDate.Value);
        }

        if (query.EndDate.HasValue)
        {
            Query.Where(tb => tb.PeriodEndDate <= query.EndDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            Query.Where(tb => tb.Status == query.Status);
        }

        if (query.IsBalanced.HasValue)
        {
            Query.Where(tb => tb.IsBalanced == query.IsBalanced.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.TrialBalanceNumber))
        {
            Query.Where(tb => tb.TrialBalanceNumber.Contains(query.TrialBalanceNumber));
        }

        Query.Skip(query.PageNumber * query.PageSize)
             .Take(query.PageSize);

        Query.OrderByDescending(tb => tb.GeneratedDate)
             .ThenBy(tb => tb.TrialBalanceNumber);
    }
}

