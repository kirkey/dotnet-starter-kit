namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Specifications;

/// <summary>
/// Specification to check if a designation code already exists.
/// Code is globally unique across all designations.
/// </summary>
public class DesignationByCodeAndOrgUnitSpec : Specification<Designation>
{
    public DesignationByCodeAndOrgUnitSpec(string code)
    {
        Query.Where(d => d.Code == code);
    }
}

