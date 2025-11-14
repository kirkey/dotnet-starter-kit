using FSH.Starter.WebApi.HumanResources.Application.Holidays.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Holidays.Search.v1;

/// <summary>
/// Request to search holidays with filtering and pagination.
/// </summary>
public class SearchHolidaysRequest : PaginationFilter, IRequest<PagedList<HolidayResponse>>
{
    /// <summary>
    /// Gets or sets the search string to filter holidays by name or description.
    /// </summary>
    public string? SearchString { get; set; }

    /// <summary>
    /// Gets or sets the start date filter.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date filter.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by paid status.
    /// </summary>
    public bool? IsPaid { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by active status.
    /// </summary>
    public bool? IsActive { get; set; }
}

