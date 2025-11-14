using FSH.Starter.WebApi.HumanResources.Application.Employees.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Search.v1;

public class SearchEmployeesRequest : PaginationFilter, IRequest<PagedList<EmployeeResponse>>
{
    public string? SearchString { get; set; }
    public DefaultIdType? OrganizationalUnitId { get; set; }
    public string? Status { get; set; }
    public bool? IsActive { get; set; }
}

