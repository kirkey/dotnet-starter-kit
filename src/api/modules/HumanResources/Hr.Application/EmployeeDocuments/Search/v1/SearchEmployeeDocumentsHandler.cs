using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Search.v1;

/// <summary>
/// Handler for searching employee documents.
/// </summary>
public sealed class SearchEmployeeDocumentsHandler(
    [FromKeyedServices("hr:documents")] IReadRepository<EmployeeDocument> repository)
    : IRequestHandler<SearchEmployeeDocumentsRequest, PagedList<EmployeeDocumentResponse>>
{
    /// <summary>
    /// Handles the request to search employee documents.
    /// </summary>
    public async Task<PagedList<EmployeeDocumentResponse>> Handle(
        SearchEmployeeDocumentsRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchEmployeeDocumentsSpec(request);
        var documents = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = documents.Select(document => new EmployeeDocumentResponse
        {
            Id = document.Id,
            EmployeeId = document.EmployeeId,
            DocumentType = document.DocumentType,
            Title = document.Title,
            FileName = document.FileName,
            FilePath = document.FilePath,
            FileSize = document.FileSize,
            ExpiryDate = document.ExpiryDate,
            IsExpired = document.IsExpired,
            DaysUntilExpiry = document.DaysUntilExpiry,
            IssueNumber = document.IssueNumber,
            IssueDate = document.IssueDate,
            UploadedDate = document.UploadedDate,
            Version = document.Version,
            Notes = document.Notes,
            IsActive = document.IsActive
        }).ToList();

        return new PagedList<EmployeeDocumentResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}
