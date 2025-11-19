using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Specifications;

/// <summary>
/// Specification to search organizational units with filters.
/// </summary>
public class SearchOrganizationalUnitsSpec : EntitiesByPaginationFilterSpec<OrganizationalUnit, OrganizationalUnitResponse>
{
    public SearchOrganizationalUnitsSpec(SearchOrganizationalUnitsRequest request)
        : base(request) =>
        Query
            .OrderBy(ou => ou.Level, !request.HasOrderBy())
            .ThenBy(ou => ou.Code)
            .Where(ou => ou.Type == request.Type, request.Type.HasValue)
            .Where(ou => ou.ParentId == request.ParentId, request.ParentId.HasValue)
            .Where(ou => ou.IsActive == request.IsActive, request.IsActive.HasValue);
}

