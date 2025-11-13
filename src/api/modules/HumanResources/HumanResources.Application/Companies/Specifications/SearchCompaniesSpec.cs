using FSH.Starter.WebApi.HumanResources.Application.Companies.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Companies.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Companies.Specifications;

/// <summary>
/// Specification to search companies with filters.
/// </summary>
public class SearchCompaniesSpec : EntitiesByPaginationFilterSpec<Company, CompanyResponse>
{
    public SearchCompaniesSpec(SearchCompaniesRequest request)
        : base(request) =>
        Query
            .OrderBy(c => c.CompanyCode, !request.HasOrderBy())
            .Where(c => c.IsActive == request.IsActive, request.IsActive.HasValue);
}

