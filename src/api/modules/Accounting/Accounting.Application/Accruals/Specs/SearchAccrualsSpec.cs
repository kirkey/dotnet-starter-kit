namespace Accounting.Application.Accruals.Specs;

public sealed class SearchAccrualsSpec : Specification<Accrual>
{
    public SearchAccrualsSpec(string? numberLike, DateTime? dateFrom, DateTime? dateTo, bool? isReversed)
    {
        if (!string.IsNullOrWhiteSpace(numberLike))
            Query.Where(a => a.AccrualNumber.Contains(numberLike));
        if (dateFrom.HasValue)
            Query.Where(a => a.AccrualDate >= dateFrom.Value);
        if (dateTo.HasValue)
            Query.Where(a => a.AccrualDate <= dateTo.Value);
        if (isReversed.HasValue)
            Query.Where(a => a.IsReversed == isReversed.Value);

        Query.OrderByDescending(a => a.AccrualDate).ThenBy(a => a.AccrualNumber);
    }
}

