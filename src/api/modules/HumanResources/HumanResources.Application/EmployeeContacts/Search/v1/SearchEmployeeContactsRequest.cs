using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Search.v1;

/// <summary>
/// Request to search employee contacts with filtering and pagination.
/// </summary>
public class SearchEmployeeContactsRequest : PaginationFilter, IRequest<PagedList<EmployeeContactResponse>>
{
    /// <summary>
    /// Gets or sets the search string to filter contacts by name or phone.
    /// </summary>
    public string? SearchString { get; set; }

    /// <summary>
    /// Gets or sets the employee ID to filter contacts for a specific employee.
    /// </summary>
    public DefaultIdType? EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the contact type filter.
    /// </summary>
    public string? ContactType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by active status.
    /// </summary>
    public bool? IsActive { get; set; }
}

