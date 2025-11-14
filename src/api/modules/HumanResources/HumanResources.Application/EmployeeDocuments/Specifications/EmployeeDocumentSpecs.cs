namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Specifications;

/// <summary>
/// Specification for getting an employee document by ID.
/// </summary>
public class EmployeeDocumentByIdSpec : Specification<EmployeeDocument>, ISingleResultSpecification<EmployeeDocument>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeDocumentByIdSpec"/> class.
    /// </summary>
    public EmployeeDocumentByIdSpec(DefaultIdType id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Employee);
    }
}

/// <summary>
/// Specification for searching employee documents with filters.
/// </summary>
public class SearchEmployeeDocumentsSpec : Specification<EmployeeDocument>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchEmployeeDocumentsSpec"/> class.
    /// </summary>
    public SearchEmployeeDocumentsSpec(Search.v1.SearchEmployeeDocumentsRequest request)
    {
        Query
            .Include(x => x.Employee)
            .OrderByDescending(x => x.UploadedDate)
            .ThenBy(x => x.Title);

        if (request.EmployeeId.HasValue)
            Query.Where(x => x.EmployeeId == request.EmployeeId);

        if (!string.IsNullOrWhiteSpace(request.SearchString))
            Query.Where(x => x.Title.Contains(request.SearchString) ||
                           x.FileName!.Contains(request.SearchString));

        if (!string.IsNullOrWhiteSpace(request.DocumentType))
            Query.Where(x => x.DocumentType == request.DocumentType);

        if (request.IsExpired.HasValue)
            Query.Where(x => x.IsExpired == request.IsExpired);

        if (request.IsActive.HasValue)
            Query.Where(x => x.IsActive == request.IsActive);
    }
}

