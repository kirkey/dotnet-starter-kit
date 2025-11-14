namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Specifications;

/// <summary>
/// Specification for getting an employee dependent by ID.
/// </summary>
public class EmployeeDependentByIdSpec : Specification<EmployeeDependent>, ISingleResultSpecification<EmployeeDependent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeDependentByIdSpec"/> class.
    /// </summary>
    public EmployeeDependentByIdSpec(DefaultIdType id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Employee);
    }
}

/// <summary>
/// Specification for searching employee dependents with filters.
/// </summary>
public class SearchEmployeeDependentsSpec : Specification<EmployeeDependent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchEmployeeDependentsSpec"/> class.
    /// </summary>
    public SearchEmployeeDependentsSpec(Search.v1.SearchEmployeeDependentsRequest request)
    {
        Query
            .Include(x => x.Employee)
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName);

        if (request.EmployeeId.HasValue)
            Query.Where(x => x.EmployeeId == request.EmployeeId);

        if (!string.IsNullOrWhiteSpace(request.SearchString))
            Query.Where(x => x.FirstName.Contains(request.SearchString) ||
                           x.LastName.Contains(request.SearchString));

        if (!string.IsNullOrWhiteSpace(request.DependentType))
            Query.Where(x => x.DependentType == request.DependentType);

        if (request.IsBeneficiary.HasValue)
            Query.Where(x => x.IsBeneficiary == request.IsBeneficiary);

        if (request.IsClaimableDependent.HasValue)
            Query.Where(x => x.IsClaimableDependent == request.IsClaimableDependent);

        if (request.IsActive.HasValue)
            Query.Where(x => x.IsActive == request.IsActive);
    }
}

