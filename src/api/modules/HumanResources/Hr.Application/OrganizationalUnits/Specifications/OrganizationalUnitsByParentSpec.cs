namespace FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Specifications;

/// <summary>
/// Specification to check if organizational unit has children.
/// </summary>
public class OrganizationalUnitsByParentSpec : Specification<OrganizationalUnit>
{
    public OrganizationalUnitsByParentSpec(DefaultIdType parentId)
    {
        Query.Where(ou => ou.ParentId == parentId);
    }
}

