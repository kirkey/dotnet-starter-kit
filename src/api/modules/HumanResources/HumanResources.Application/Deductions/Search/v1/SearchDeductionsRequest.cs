using FSH.Starter.WebApi.HumanResources.Application.Deductions.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Search.v1;

/// <summary>
/// Request to search deductions with filtering and pagination.
/// </summary>
public class SearchDeductionsRequest : PaginationFilter, IRequest<PagedList<DeductionResponse>>
{
    /// <summary>
    /// Gets or sets the search string for deduction name.
    /// </summary>
    public string? SearchString { get; set; }

    /// <summary>
    /// Gets or sets the component type filter (Deduction, Tax, Earnings).
    /// </summary>
    public string? ComponentType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by active deductions only.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by calculated deductions.
    /// </summary>
    public bool? IsCalculated { get; set; }
}

