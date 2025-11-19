namespace FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Search.v1;

using FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Get.v1;

/// <summary>
/// Request to search pay component rates with filtering and pagination.
/// </summary>
public sealed class SearchPayComponentRatesRequest : PaginationFilter, IRequest<PagedList<PayComponentRateResponse>>
{
    /// <summary>
    /// Filter by pay component ID.
    /// </summary>
    public DefaultIdType? PayComponentId { get; set; }

    /// <summary>
    /// Filter by year (e.g., 2025).
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// Filter by minimum amount range.
    /// </summary>
    public decimal? MinAmountFrom { get; set; }

    /// <summary>
    /// Filter by maximum amount range.
    /// </summary>
    public decimal? MaxAmountTo { get; set; }

    /// <summary>
    /// Filter by active status.
    /// </summary>
    public bool? IsActive { get; set; }
}

