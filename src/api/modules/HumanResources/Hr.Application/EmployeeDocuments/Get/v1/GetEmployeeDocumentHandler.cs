using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Get.v1;

/// <summary>
/// Handler for retrieving an employee document by ID.
/// </summary>
public sealed class GetEmployeeDocumentHandler(
    [FromKeyedServices("hr:documents")] IReadRepository<EmployeeDocument> repository)
    : IRequestHandler<GetEmployeeDocumentRequest, EmployeeDocumentResponse>
{
    /// <summary>
    /// Handles the request to get an employee document.
    /// </summary>
    public async Task<EmployeeDocumentResponse> Handle(
        GetEmployeeDocumentRequest request,
        CancellationToken cancellationToken)
    {
        var document = await repository
            .FirstOrDefaultAsync(new EmployeeDocumentByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (document is null)
            throw new EmployeeDocumentNotFoundException(request.Id);

        return new EmployeeDocumentResponse
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
        };
    }
}
