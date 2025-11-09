namespace Accounting.Application.TrialBalance.Search.v1;

/// <summary>
/// Specification for searching trial balance reports.
/// </summary>
public sealed class TrialBalanceSearchSpec : Specification<Domain.Entities.TrialBalance>
{
    public TrialBalanceSearchSpec(TrialBalanceSearchRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.PeriodId.HasValue && request.PeriodId.Value != DefaultIdType.Empty)
        {
            Query.Where(tb => tb.PeriodId == request.PeriodId.Value);
        }

        if (request.StartDate.HasValue)
        {
            Query.Where(tb => tb.PeriodStartDate >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            Query.Where(tb => tb.PeriodEndDate <= request.EndDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            Query.Where(tb => tb.Status == request.Status);
        }

        if (request.IsBalanced.HasValue)
        {
            Query.Where(tb => tb.IsBalanced == request.IsBalanced.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.TrialBalanceNumber))
        {
            Query.Where(tb => tb.TrialBalanceNumber.Contains(request.TrialBalanceNumber));
        }

        Query.Skip(request.PageNumber * request.PageSize)
             .Take(request.PageSize);

        Query.OrderByDescending(tb => tb.GeneratedDate)
             .ThenBy(tb => tb.TrialBalanceNumber);
    }
}



