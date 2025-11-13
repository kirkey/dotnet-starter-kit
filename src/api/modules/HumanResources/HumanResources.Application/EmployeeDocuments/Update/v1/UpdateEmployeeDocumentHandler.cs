namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Update.v1;

public sealed class UpdateEmployeeDocumentHandler(
    ILogger<UpdateEmployeeDocumentHandler> logger,
    [FromKeyedServices("hr:documents")] IRepository<EmployeeDocument> repository)
    : IRequestHandler<UpdateEmployeeDocumentCommand, UpdateEmployeeDocumentResponse>
{
    public async Task<UpdateEmployeeDocumentResponse> Handle(
        UpdateEmployeeDocumentCommand request,
        CancellationToken cancellationToken)
    {
        var document = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (document is null)
            throw new EmployeeDocumentNotFoundException(request.Id);

        document.Update(
            request.Title,
            request.ExpiryDate,
            request.IssueNumber,
            request.IssueDate,
            request.Notes);

        if (!string.IsNullOrWhiteSpace(request.FileName) && !string.IsNullOrWhiteSpace(request.FilePath) && request.FileSize.HasValue)
            document.ReplaceFile(request.FileName, request.FilePath, request.FileSize.Value);

        await repository.UpdateAsync(document, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Employee document {DocumentId} updated successfully", document.Id);

        return new UpdateEmployeeDocumentResponse(document.Id);
    }
}

