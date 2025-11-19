namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Specifications;

/// <summary>
/// Specification to get designation by ID.
/// </summary>
public class DesignationByIdSpec : Specification<Designation>
{
    public DesignationByIdSpec(DefaultIdType id)
    {
        Query
            .Where(d => d.Id == id);
    }
}

