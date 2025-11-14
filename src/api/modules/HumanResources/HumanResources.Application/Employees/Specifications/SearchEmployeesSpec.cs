using FSH.Starter.WebApi.HumanResources.Application.Employees.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Employees.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Specifications;

public class SearchEmployeesSpec : EntitiesByPaginationFilterSpec<Employee, EmployeeResponse>
{
    public SearchEmployeesSpec(SearchEmployeesRequest request)
        : base(request) =>
        Query
            .Where(e => e.EmployeeNumber.Contains(request.SearchString!) || 
                        e.FirstName.Contains(request.SearchString!) || 
                        e.LastName.Contains(request.SearchString!) || 
                        e.Email!.Contains(request.SearchString!), !string.IsNullOrWhiteSpace(request.SearchString))
            .Where(e => e.OrganizationalUnitId == request.OrganizationalUnitId, request.OrganizationalUnitId.HasValue)
            .Where(e => e.Status == request.Status, !string.IsNullOrWhiteSpace(request.Status))
            .Where(e => e.IsActive == request.IsActive, request.IsActive.HasValue)
            .OrderBy(e => e.EmployeeNumber, !request.HasOrderBy());
}

