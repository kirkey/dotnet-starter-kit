using Accounting.Application.Consumptions.Responses;

namespace Accounting.Application.Consumptions.Queries;

/// <summary>
/// Query used to search Consumptions with optional filters and pagination.
/// </summary>
public class SearchConsumptionQuery : PaginationFilter, IRequest<PagedList<ConsumptionResponse>>
{
    /// <summary>
    /// Optional meter identifier to filter consumptions by a specific meter.
    /// </summary>
    public DefaultIdType? MeterId { get; set; }

    /// <summary>
    /// Optional start date (inclusive) to filter consumption reading dates.
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// Optional end date (inclusive) to filter consumption reading dates.
    /// </summary>
    public DateTime? ToDate { get; set; }

    /// <summary>
    /// Optional billing period text to filter by billing period (partial match).
    /// Example: "2025-08" or "Aug-2025" depending on stored format.
    /// </summary>
    public string? BillingPeriod { get; set; }
}
