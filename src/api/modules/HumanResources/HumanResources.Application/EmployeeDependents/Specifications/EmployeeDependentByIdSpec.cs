namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Specifications;

/// <summary>
/// Specification to get employee dependent by ID.
/// </summary>
public class EmployeeDependentByIdSpec : Specification<EmployeeDependent>
{
    public EmployeeDependentByIdSpec(DefaultIdType id)
    {
        Query.Where(d => d.Id == id);
    }
}

