using FSH.Starter.WebApi.HumanResources.Application.Companies.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.Companies.Search.v1;

/// <summary>
/// Request to search companies.
/// </summary>
public class SearchCompaniesRequest : PaginationFilter, IRequest<PagedList<CompanyResponse>>
{
    public bool? IsActive { get; set; }
}

