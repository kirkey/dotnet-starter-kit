namespace FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Specifications;

/// <summary>
/// Specification for searching employee pay components with filters.
/// </summary>
public sealed class SearchEmployeePayComponentsSpec : Specification<EmployeePayComponent>
{
    public SearchEmployeePayComponentsSpec(Search.v1.SearchEmployeePayComponentsRequest request)
    {
        Query
            .Include(x => x.Employee)
            .Include(x => x.PayComponent)
            .OrderBy(x => x.EmployeeId)
            .ThenBy(x => x.PayComponentId);

        if (request.EmployeeId.HasValue)
            Query.Where(x => x.EmployeeId == request.EmployeeId);

        if (request.PayComponentId.HasValue)
            Query.Where(x => x.PayComponentId == request.PayComponentId);

        if (!string.IsNullOrWhiteSpace(request.AssignmentType))
            Query.Where(x => x.AssignmentType == request.AssignmentType);

        if (request.IsActive.HasValue)
            Query.Where(x => x.IsActive == request.IsActive);

        if (request.IsRecurring.HasValue)
            Query.Where(x => x.IsRecurring == request.IsRecurring);
    }
}

/// <summary>
/// Specification for getting an employee pay component by ID with includes.
/// </summary>
public sealed class EmployeePayComponentByIdSpec : Specification<EmployeePayComponent>, ISingleResultSpecification<EmployeePayComponent>
{
    public EmployeePayComponentByIdSpec(DefaultIdType id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Employee)
            .Include(x => x.PayComponent);
    }
}

