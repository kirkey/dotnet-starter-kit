namespace FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Specifications;

/// <summary>
/// Specification to get organizational unit by code within a company.
/// </summary>
public class OrganizationalUnitByCodeSpec : Specification<OrganizationalUnit>
{
    public OrganizationalUnitByCodeSpec(DefaultIdType companyId, string code)
    {
        Query.Where(ou => ou.CompanyId == companyId && ou.Code == code);
    }
}

