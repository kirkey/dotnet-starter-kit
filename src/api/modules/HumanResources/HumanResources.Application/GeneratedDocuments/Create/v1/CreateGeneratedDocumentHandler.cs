namespace FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Create.v1;

public sealed class CreateGeneratedDocumentHandler(
    ILogger<CreateGeneratedDocumentHandler> logger,
    [FromKeyedServices("hr:generateddocuments")] IRepository<GeneratedDocument> repository)
    : IRequestHandler<CreateGeneratedDocumentCommand, CreateGeneratedDocumentResponse>
{
    public async Task<CreateGeneratedDocumentResponse> Handle(
        CreateGeneratedDocumentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var document = GeneratedDocument.Create(
            request.DocumentTemplateId,
            request.EntityId,
            request.EntityType,
            request.GeneratedContent);

        if (!string.IsNullOrWhiteSpace(request.Notes))
            document.AddNotes(request.Notes);

        await repository.AddAsync(document, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Generated document created with ID {DocumentId}, Entity {EntityType} {EntityId}",
            document.Id,
            document.EntityType,
            document.EntityId);

        return new CreateGeneratedDocumentResponse(document.Id);
    }
}

