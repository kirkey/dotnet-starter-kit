namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Specifications;

/// <summary>
/// Specification to get designation by code.
/// Code is globally unique across all designations.
/// </summary>
public class DesignationByCodeSpec : Specification<Designation>
{
    public DesignationByCodeSpec(string code)
    {
        Query.Where(d => d.Code == code);
    }
}

