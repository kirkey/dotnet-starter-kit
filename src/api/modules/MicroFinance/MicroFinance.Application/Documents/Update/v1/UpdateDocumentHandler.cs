using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.Update.v1;

/// <summary>
/// Handler for updating a document.
/// </summary>
public sealed class UpdateDocumentHandler(
    ILogger<UpdateDocumentHandler> logger,
    [FromKeyedServices("microfinance:documents")] IRepository<Document> repository)
    : IRequestHandler<UpdateDocumentCommand, UpdateDocumentResponse>
{
    /// <summary>
    /// Handles the update document command.
    /// </summary>
    public async Task<UpdateDocumentResponse> Handle(
        UpdateDocumentCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var document = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new KeyNotFoundException($"Document with ID {request.Id} not found.");

        document.Update(
            request.Name,
            request.Description,
            request.Tags,
            request.DisplayOrder);

        await repository.UpdateAsync(document, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Document {Name} updated", document.Name);

        return new UpdateDocumentResponse(
            document.Id,
            document.Name,
            document.DocumentType,
            document.Description,
            document.Status);
    }
}
