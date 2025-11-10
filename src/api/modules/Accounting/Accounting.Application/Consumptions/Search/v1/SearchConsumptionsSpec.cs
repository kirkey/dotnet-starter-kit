using Accounting.Application.Consumptions.Responses;

namespace Accounting.Application.Consumptions.Search.v1;

/// <summary>
/// Specification for searching consumption records with filtering and pagination.
/// Projects results to <see cref="ConsumptionResponse"/>.
/// </summary>
public sealed class SearchConsumptionsSpec : EntitiesByPaginationFilterSpec<Consumption, ConsumptionResponse>
{
    public SearchConsumptionsSpec(SearchConsumptionsRequest request) : base(request)
    {
        Query
            .OrderByDescending(c => c.ReadingDate, !request.HasOrderBy())
            .Where(c => c.MeterId == request.MeterId!.Value, request.MeterId.HasValue)
            .Where(c => c.ReadingDate >= request.ReadingDateFrom!.Value, request.ReadingDateFrom.HasValue)
            .Where(c => c.ReadingDate <= request.ReadingDateTo!.Value, request.ReadingDateTo.HasValue)
            .Where(c => c.BillingPeriod == request.BillingPeriod!, !string.IsNullOrWhiteSpace(request.BillingPeriod))
            .Where(c => c.ReadingType == request.ReadingType!, !string.IsNullOrWhiteSpace(request.ReadingType))
            .Where(c => c.IsValidReading == request.IsValidReading!.Value, request.IsValidReading.HasValue);
    }
}

