using Ardalis.Specification;

namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Specifications;

/// <summary>
/// Specification to get designation by code and organizational unit.
/// </summary>
public class DesignationByCodeAndOrgUnitSpec : Specification<Designation>
{
    public DesignationByCodeAndOrgUnitSpec(DefaultIdType organizationalUnitId, string code)
    {
        Query.Where(d => d.OrganizationalUnitId == organizationalUnitId && d.Code == code);
    }
}

