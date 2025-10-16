using Accounting.Application.Consumptions.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.Consumptions.Queries;

/// <summary>
/// Specification for searching consumptions with filtering and pagination.
/// </summary>
public class SearchConsumptionSpec : EntitiesByPaginationFilterSpec<Consumption, ConsumptionResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchConsumptionSpec"/> class.
    /// </summary>
    /// <param name="request">The search consumption query containing filter criteria and pagination parameters.</param>
    public SearchConsumptionSpec(SearchConsumptionQuery request)
        : base(request)
    {
        Query
            .Where(x => x.MeterId == request.MeterId!.Value, request.MeterId.HasValue)
            .Where(x => x.BillingPeriod.Contains(request.BillingPeriod!), !string.IsNullOrWhiteSpace(request.BillingPeriod))
            .Where(x => x.ReadingDate >= request.FromDate!.Value, request.FromDate.HasValue)
            .Where(x => x.ReadingDate <= request.ToDate!.Value, request.ToDate.HasValue)
            .OrderByDescending(x => x.ReadingDate, !request.HasOrderBy())
            .ThenBy(x => x.MeterId);
    }
}

