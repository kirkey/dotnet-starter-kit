using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Specifications;

/// <summary>
/// Specification to get organizational unit by ID with parent.
/// </summary>
public class OrganizationalUnitByIdSpec : Specification<OrganizationalUnit, OrganizationalUnitResponse>
{
    public OrganizationalUnitByIdSpec(DefaultIdType id)
    {
        Query
            .Where(ou => ou.Id == id)
            .Include(ou => ou.Parent);
    }
}

