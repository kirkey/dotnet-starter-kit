using FSH.Starter.WebApi.HumanResources.Application.Taxes.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Search.v1;

/// <summary>
/// Request to search tax brackets with filtering and pagination.
/// </summary>
public class SearchTaxesRequest : PaginationFilter, IRequest<PagedList<TaxResponse>>
{
    /// <summary>
    /// Gets or sets the tax type filter (Federal, State, FICA, etc).
    /// </summary>
    public string? TaxType { get; set; }

    /// <summary>
    /// Gets or sets the year filter.
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// Gets or sets the filing status filter (Single, Married, etc).
    /// </summary>
    public string? FilingStatus { get; set; }

    /// <summary>
    /// Gets or sets the income range start (minimum for filtering).
    /// </summary>
    public decimal? MinIncomeFilter { get; set; }

    /// <summary>
    /// Gets or sets the income range end (maximum for filtering).
    /// </summary>
    public decimal? MaxIncomeFilter { get; set; }
}

