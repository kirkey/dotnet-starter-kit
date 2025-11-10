using Accounting.Application.Consumptions.Responses;

namespace Accounting.Application.Consumptions.Search.v1;

/// <summary>
/// Request to search for consumption records with optional filters and pagination.
/// </summary>
public sealed class SearchConsumptionsRequest : PaginationFilter, IRequest<PagedList<ConsumptionResponse>>
{
    public DefaultIdType? MeterId { get; init; }
    public DateTime? ReadingDateFrom { get; init; }
    public DateTime? ReadingDateTo { get; init; }
    public string? BillingPeriod { get; init; }
    public string? ReadingType { get; init; }
    public bool? IsValidReading { get; init; }
}

