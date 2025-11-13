namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Specifications;

/// <summary>
/// Specification to get employee contact by ID.
/// </summary>
public class EmployeeContactByIdSpec : Specification<EmployeeContact>
{
    public EmployeeContactByIdSpec(DefaultIdType id)
    {
        Query.Where(c => c.Id == id);
    }
}

