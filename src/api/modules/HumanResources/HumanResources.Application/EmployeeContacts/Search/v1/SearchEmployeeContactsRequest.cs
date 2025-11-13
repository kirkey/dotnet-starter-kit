using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Search.v1;

/// <summary>
/// Request to search employee contacts.
/// </summary>
public class SearchEmployeeContactsRequest : PaginationFilter, IRequest<PagedList<EmployeeContactResponse>>
{
    public DefaultIdType? EmployeeId { get; set; }
    public string? ContactType { get; set; }
    public bool? IsActive { get; set; }
}

