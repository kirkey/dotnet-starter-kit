namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Update.v1;

/// <summary>
/// Handler for updating an employee document.
/// </summary>
public sealed class UpdateEmployeeDocumentHandler(
    ILogger<UpdateEmployeeDocumentHandler> logger,
    [FromKeyedServices("hr:documents")] IRepository<EmployeeDocument> repository)
    : IRequestHandler<UpdateEmployeeDocumentCommand, UpdateEmployeeDocumentResponse>
{
    /// <summary>
    /// Handles the request to update an employee document.
    /// </summary>
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

        await repository.UpdateAsync(document, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Employee document {DocumentId} updated successfully", document.Id);

        return new UpdateEmployeeDocumentResponse(document.Id);
    }
}

