using FSH.Starter.WebApi.HumanResources.Application.Designations.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Designations.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Specifications;

/// <summary>
/// Specification to search designations with filters by area, salary grade, status, and salary ranges.
/// </summary>
public class SearchDesignationsSpec : EntitiesByPaginationFilterSpec<Designation, DesignationResponse>
{
    public SearchDesignationsSpec(SearchDesignationsRequest request)
        : base(request) =>
        Query
            .OrderBy(d => d.Code, !request.HasOrderBy())
            .Where(d => d.Area == request.Area, !string.IsNullOrWhiteSpace(request.Area))
            .Where(d => d.SalaryGrade == request.SalaryGrade, !string.IsNullOrWhiteSpace(request.SalaryGrade))
            .Where(d => d.IsActive == request.IsActive, request.IsActive.HasValue)
            .Where(d => d.IsManagerial == request.IsManagerial, request.IsManagerial.HasValue)
            .Where(d => d.MinimumSalary >= request.SalaryMin, request.SalaryMin.HasValue)
            .Where(d => d.MaximumSalary <= request.SalaryMax, request.SalaryMax.HasValue)
            .Where(d => d.Title.Contains(request.Keyword!) || d.Code.Contains(request.Keyword!), !string.IsNullOrWhiteSpace(request.Keyword));
}

