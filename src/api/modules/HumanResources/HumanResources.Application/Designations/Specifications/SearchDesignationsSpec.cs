using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.HumanResources.Application.Designations.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Designations.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Specifications;

/// <summary>
/// Specification to search designations with filters.
/// </summary>
public class SearchDesignationsSpec : EntitiesByPaginationFilterSpec<Designation, DesignationResponse>
{
    public SearchDesignationsSpec(SearchDesignationsRequest request)
        : base(request) =>
        Query
            .Include(d => d.OrganizationalUnit)
            .OrderBy(d => d.Code, !request.HasOrderBy())
            .Where(d => d.OrganizationalUnitId == request.OrganizationalUnitId, request.OrganizationalUnitId.HasValue)
            .Where(d => d.Title.Contains(request.Title!), !string.IsNullOrWhiteSpace(request.Title))
            .Where(d => d.IsActive == request.IsActive, request.IsActive.HasValue)
            .Where(d => d.MinSalary >= request.SalaryMin, request.SalaryMin.HasValue)
            .Where(d => d.MaxSalary <= request.SalaryMax, request.SalaryMax.HasValue);
}

