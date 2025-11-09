using Accounting.Application.Accruals.Responses;

namespace Accounting.Application.Accruals.Specs;
using Search;

/// <summary>
/// Specification for filtering accruals based on search query parameters.
/// </summary>
public sealed class SearchAccrualsSpec : Specification<Accrual, AccrualResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchAccrualsSpec"/> class using the provided query.
    /// </summary>
    /// <param name="request">The search query containing filter parameters.</param>
    public SearchAccrualsSpec(SearchAccrualsRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.NumberLike))
            Query.Where(a => a.AccrualNumber.Contains(request.NumberLike));
        if (request.DateFrom.HasValue)
            Query.Where(a => a.AccrualDate >= request.DateFrom.Value);
        if (request.DateTo.HasValue)
            Query.Where(a => a.AccrualDate <= request.DateTo.Value);
        if (request.IsReversed.HasValue)
            Query.Where(a => a.IsReversed == request.IsReversed.Value);

        Query.OrderByDescending(a => a.AccrualDate).ThenBy(a => a.AccrualNumber);
    }
}
