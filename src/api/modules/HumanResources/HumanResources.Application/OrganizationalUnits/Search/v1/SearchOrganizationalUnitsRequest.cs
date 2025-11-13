using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Search.v1;

/// <summary>
/// Request to search organizational units.
/// </summary>
public class SearchOrganizationalUnitsRequest : PaginationFilter, IRequest<PagedList<OrganizationalUnitResponse>>
{
    public DefaultIdType? CompanyId { get; set; }
    public OrganizationalUnitType? Type { get; set; }
    public DefaultIdType? ParentId { get; set; }
    public bool? IsActive { get; set; }
}

