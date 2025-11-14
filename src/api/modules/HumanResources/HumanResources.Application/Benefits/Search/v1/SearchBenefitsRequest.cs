using FSH.Starter.WebApi.HumanResources.Application.Benefits.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Search.v1;

/// <summary>
/// Request to search benefits with filtering and pagination.
/// </summary>
public class SearchBenefitsRequest : PaginationFilter, IRequest<PagedList<BenefitResponse>>
{
    /// <summary>
    /// Gets or sets the search string for benefit name.
    /// </summary>
    public string? SearchString { get; set; }

    /// <summary>
    /// Gets or sets the benefit type filter (Health, Retirement, Life Insurance, etc).
    /// </summary>
    public string? BenefitType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by active benefits only.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by required benefits.
    /// </summary>
    public bool? IsRequired { get; set; }
}

