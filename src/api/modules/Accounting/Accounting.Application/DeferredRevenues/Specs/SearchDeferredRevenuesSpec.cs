using DeferredRevenueEntity = Accounting.Domain.Entities.DeferredRevenue;

namespace Accounting.Application.DeferredRevenues.Specs;

/// <summary>
/// Specification for filtering deferred revenues based on search query parameters.
/// </summary>
public sealed class SearchDeferredRevenuesSpec : Specification<DeferredRevenueEntity>
{
    public SearchDeferredRevenuesSpec(Search.SearchDeferredRevenuesRequest request)
    {
        Query
            .Where(d => d.DeferredRevenueNumber.Contains(request.DeferredRevenueNumber!), !string.IsNullOrWhiteSpace(request.DeferredRevenueNumber))
            .Where(d => d.IsRecognized == request.IsRecognized, request.IsRecognized.HasValue)
            .Where(d => d.RecognitionDate >= request.RecognitionDateFrom, request.RecognitionDateFrom.HasValue)
            .Where(d => d.RecognitionDate <= request.RecognitionDateTo, request.RecognitionDateTo.HasValue);

        Query
            .OrderByDescending(d => d.RecognitionDate)
            .ThenBy(d => d.DeferredRevenueNumber);
    }
}
