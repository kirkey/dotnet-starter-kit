using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.Create.v1;

/// <summary>
/// Handler for creating a new document record.
/// </summary>
public sealed class CreateDocumentHandler(
    [FromKeyedServices("microfinance:documents")] IRepository<Document> repository,
    ILogger<CreateDocumentHandler> logger)
    : IRequestHandler<CreateDocumentCommand, CreateDocumentResponse>
{
    public async Task<CreateDocumentResponse> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = Document.Create(
            request.Name,
            request.DocumentType,
            request.EntityType,
            request.EntityId,
            request.FilePath,
            request.FileSizeBytes,
            request.MimeType,
            request.Category,
            request.Description,
            request.OriginalFileName);

        await repository.AddAsync(document, cancellationToken);

        logger.LogInformation("Document {DocumentId} created - Name: {Name}, Type: {Type} for {EntityType} {EntityId}",
            document.Id, request.Name, request.DocumentType, request.EntityType, request.EntityId);

        return new CreateDocumentResponse(
            document.Id,
            document.Name,
            document.DocumentType,
            document.EntityType,
            document.EntityId);
    }
}
