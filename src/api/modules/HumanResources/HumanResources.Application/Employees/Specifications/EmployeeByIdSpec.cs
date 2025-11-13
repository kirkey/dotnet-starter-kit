namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Specifications;

/// <summary>
/// Specification to get employee by ID.
/// </summary>
public class EmployeeByIdSpec : Specification<Employee>
{
    public EmployeeByIdSpec(DefaultIdType id)
    {
        Query
            .Where(e => e.Id == id)
            .Include(e => e.OrganizationalUnit);
    }
}

