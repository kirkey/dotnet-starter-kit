using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.HumanResources.Application.Designations.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Search.v1;

/// <summary>
/// Request to search designations.
/// </summary>
public class SearchDesignationsRequest : PaginationFilter, IRequest<PagedList<DesignationResponse>>
{
    public DefaultIdType? OrganizationalUnitId { get; set; }
    public string? Title { get; set; }
    public bool? IsActive { get; set; }
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
}

