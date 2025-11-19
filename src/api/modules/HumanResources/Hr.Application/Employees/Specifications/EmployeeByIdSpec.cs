namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Specifications;

public class EmployeeByIdSpec : Specification<Employee>
{
    public EmployeeByIdSpec(DefaultIdType id)
    {
        Query.Where(e => e.Id == id);
    }
}

