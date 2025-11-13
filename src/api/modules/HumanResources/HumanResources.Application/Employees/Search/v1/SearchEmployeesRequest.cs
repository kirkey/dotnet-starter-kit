using FSH.Starter.WebApi.HumanResources.Application.Employees.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Search.v1;

/// <summary>
/// Request to search employees.
/// </summary>
public class SearchEmployeesRequest : PaginationFilter, IRequest<PagedList<EmployeeResponse>>
{
    public DefaultIdType? OrganizationalUnitId { get; set; }
    public string? Status { get; set; }
    public bool? IsActive { get; set; }
}

