using FSH.Starter.WebApi.HumanResources.Application.Employees.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Employees.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Specifications;

/// <summary>
/// Specification to search employees with filters.
/// </summary>
public class SearchEmployeesSpec : EntitiesByPaginationFilterSpec<Employee, EmployeeResponse>
{
    public SearchEmployeesSpec(SearchEmployeesRequest request)
        : base(request) =>
        Query
            .Include(e => e.OrganizationalUnit)
            .OrderBy(e => e.EmployeeNumber, !request.HasOrderBy())
            .Where(e => e.OrganizationalUnitId == request.OrganizationalUnitId, request.OrganizationalUnitId.HasValue)
            .Where(e => e.Status == request.Status, !string.IsNullOrWhiteSpace(request.Status))
            .Where(e => e.IsActive == request.IsActive, request.IsActive.HasValue);
}

