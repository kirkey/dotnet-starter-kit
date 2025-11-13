namespace FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Specifications;

/// <summary>
/// Specification to get organizational unit by code within a company.
/// </summary>
public class OrganizationalUnitByCodeSpec : Specification<OrganizationalUnit>
{
    public OrganizationalUnitByCodeSpec(string code)
    {
        Query.Where(ou => ou.Code == code);
    }
}

