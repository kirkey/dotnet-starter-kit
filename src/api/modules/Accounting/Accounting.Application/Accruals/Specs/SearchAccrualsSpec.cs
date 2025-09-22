using Accounting.Application.Accruals.Responses;

namespace Accounting.Application.Accruals.Specs;
using Accounting.Application.Accruals.Search;

/// <summary>
/// Specification for filtering accruals based on search query parameters.
/// </summary>
public sealed class SearchAccrualsSpec : Specification<Accrual, AccrualResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchAccrualsSpec"/> class using the provided query.
    /// </summary>
    /// <param name="query">The search query containing filter parameters.</param>
    public SearchAccrualsSpec(SearchAccrualsQuery query)
    {
        if (!string.IsNullOrWhiteSpace(query.NumberLike))
            Query.Where(a => a.AccrualNumber.Contains(query.NumberLike));
        if (query.DateFrom.HasValue)
            Query.Where(a => a.AccrualDate >= query.DateFrom.Value);
        if (query.DateTo.HasValue)
            Query.Where(a => a.AccrualDate <= query.DateTo.Value);
        if (query.IsReversed.HasValue)
            Query.Where(a => a.IsReversed == query.IsReversed.Value);

        Query.OrderByDescending(a => a.AccrualDate).ThenBy(a => a.AccrualNumber);
    }
}
