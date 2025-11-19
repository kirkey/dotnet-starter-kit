namespace FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Delete.v1;

public sealed class DeleteGeneratedDocumentHandler(
    ILogger<DeleteGeneratedDocumentHandler> logger,
    [FromKeyedServices("hr:generateddocuments")] IRepository<GeneratedDocument> repository)
    : IRequestHandler<DeleteGeneratedDocumentCommand, DeleteGeneratedDocumentResponse>
{
    public async Task<DeleteGeneratedDocumentResponse> Handle(
        DeleteGeneratedDocumentCommand request,
        CancellationToken cancellationToken)
    {
        var document = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (document is null)
            throw new GeneratedDocumentNotFoundException(request.Id);

        await repository.DeleteAsync(document, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Generated document {DocumentId} deleted successfully", document.Id);

        return new DeleteGeneratedDocumentResponse(document.Id);
    }
}

