namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Delete.v1;

/// <summary>
/// Handler for deleting an employee document.
/// </summary>
public sealed class DeleteEmployeeDocumentHandler(
    ILogger<DeleteEmployeeDocumentHandler> logger,
    [FromKeyedServices("hr:documents")] IRepository<EmployeeDocument> repository)
    : IRequestHandler<DeleteEmployeeDocumentCommand, DeleteEmployeeDocumentResponse>
{
    /// <summary>
    /// Handles the request to delete an employee document.
    /// </summary>
    public async Task<DeleteEmployeeDocumentResponse> Handle(
        DeleteEmployeeDocumentCommand request,
        CancellationToken cancellationToken)
    {
        var document = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (document is null)
            throw new EmployeeDocumentNotFoundException(request.Id);

        await repository.DeleteAsync(document, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Employee document {DocumentId} deleted successfully", document.Id);

        return new DeleteEmployeeDocumentResponse(document.Id);
    }
}
