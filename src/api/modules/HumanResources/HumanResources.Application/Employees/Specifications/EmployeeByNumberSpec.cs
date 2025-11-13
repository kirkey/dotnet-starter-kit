namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Specifications;

/// <summary>
/// Specification to get employee by employee number.
/// </summary>
public class EmployeeByNumberSpec : Specification<Employee>
{
    public EmployeeByNumberSpec(string employeeNumber)
    {
        Query.Where(e => e.EmployeeNumber == employeeNumber);
    }
}

