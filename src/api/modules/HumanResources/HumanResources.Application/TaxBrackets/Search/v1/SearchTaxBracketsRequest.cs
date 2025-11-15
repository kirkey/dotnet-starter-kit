namespace FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Search.v1;

using FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Get.v1;

/// <summary>
/// Request to search tax brackets with filtering and pagination.
/// </summary>
public sealed class SearchTaxBracketsRequest : PaginationFilter, IRequest<PagedList<TaxBracketResponse>>
{
    /// <summary>
    /// Filter by tax type (Federal, State, FICA, etc).
    /// </summary>
    public string? TaxType { get; set; }

    /// <summary>
    /// Filter by year.
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// Filter by filing status (Single, Married, Head of Household, etc).
    /// </summary>
    public string? FilingStatus { get; set; }

    /// <summary>
    /// Filter by income range (min).
    /// </summary>
    public decimal? IncomeFrom { get; set; }

    /// <summary>
    /// Filter by income range (max).
    /// </summary>
    public decimal? IncomeTo { get; set; }
}

