namespace FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Update.v1;

public sealed class UpdateGeneratedDocumentHandler(
    ILogger<UpdateGeneratedDocumentHandler> logger,
    [FromKeyedServices("hr:generateddocuments")] IRepository<GeneratedDocument> repository)
    : IRequestHandler<UpdateGeneratedDocumentCommand, UpdateGeneratedDocumentResponse>
{
    public async Task<UpdateGeneratedDocumentResponse> Handle(
        UpdateGeneratedDocumentCommand request,
        CancellationToken cancellationToken)
    {
        var document = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (document is null)
            throw new GeneratedDocumentNotFoundException(request.Id);

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            switch (request.Status)
            {
                case "Finalized":
                    document.Finalize();
                    break;
                case "Signed":
                    if (string.IsNullOrWhiteSpace(request.SignedBy))
                        throw new ArgumentException("Signed by is required for signing.");
                    document.RecordSignature(request.SignedBy);
                    break;
                case "Archived":
                    document.Archive();
                    break;
            }
        }

        if (!string.IsNullOrWhiteSpace(request.FilePath))
            document.SetFilePath(request.FilePath);

        if (!string.IsNullOrWhiteSpace(request.Notes))
            document.AddNotes(request.Notes);

        await repository.UpdateAsync(document, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Generated document {DocumentId} updated successfully, Status: {Status}", document.Id, document.Status);

        return new UpdateGeneratedDocumentResponse(document.Id);
    }
}

