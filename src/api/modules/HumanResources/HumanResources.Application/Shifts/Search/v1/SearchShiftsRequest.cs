using FSH.Starter.WebApi.HumanResources.Application.Shifts.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Shifts.Search.v1;

/// <summary>
/// Request to search shifts with filtering and pagination.
/// </summary>
public class SearchShiftsRequest : PaginationFilter, IRequest<PagedList<ShiftResponse>>
{
    /// <summary>
    /// Gets or sets the search string to filter shifts by name or description.
    /// </summary>
    public string? SearchString { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by active status.
    /// </summary>
    public bool? IsActive { get; set; }
}

