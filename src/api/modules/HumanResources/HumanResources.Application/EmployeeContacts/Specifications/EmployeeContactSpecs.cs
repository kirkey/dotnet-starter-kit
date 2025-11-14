namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Specifications;

/// <summary>
/// Specification for getting an employee contact by ID.
/// </summary>
public class EmployeeContactByIdSpec : Specification<EmployeeContact>, ISingleResultSpecification<EmployeeContact>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeContactByIdSpec"/> class.
    /// </summary>
    public EmployeeContactByIdSpec(DefaultIdType id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Employee);
    }
}

/// <summary>
/// Specification for searching employee contacts with filters.
/// </summary>
public class SearchEmployeeContactsSpec : Specification<EmployeeContact>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchEmployeeContactsSpec"/> class.
    /// </summary>
    public SearchEmployeeContactsSpec(Search.v1.SearchEmployeeContactsRequest request)
    {
        Query
            .Include(x => x.Employee)
            .OrderBy(x => x.Priority)
            .ThenBy(x => x.FirstName);

        if (request.EmployeeId.HasValue)
            Query.Where(x => x.EmployeeId == request.EmployeeId);

        if (!string.IsNullOrWhiteSpace(request.SearchString))
            Query.Where(x => x.FirstName.Contains(request.SearchString) ||
                           x.LastName.Contains(request.SearchString) ||
                           x.PhoneNumber!.Contains(request.SearchString) ||
                           x.Email!.Contains(request.SearchString));

        if (!string.IsNullOrWhiteSpace(request.ContactType))
            Query.Where(x => x.ContactType == request.ContactType);

        if (request.IsActive.HasValue)
            Query.Where(x => x.IsActive == request.IsActive);
    }
}

