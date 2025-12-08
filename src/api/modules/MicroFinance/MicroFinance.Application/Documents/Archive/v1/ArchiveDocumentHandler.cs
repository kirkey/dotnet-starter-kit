using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.Archive.v1;

/// <summary>
/// Handler for archiving a document.
/// </summary>
public sealed class ArchiveDocumentHandler(
    ILogger<ArchiveDocumentHandler> logger,
    [FromKeyedServices("microfinance:documents")] IRepository<Document> repository)
    : IRequestHandler<ArchiveDocumentCommand, ArchiveDocumentResponse>
{
    /// <summary>
    /// Handles the archive document command.
    /// </summary>
    public async Task<ArchiveDocumentResponse> Handle(
        ArchiveDocumentCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var document = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new KeyNotFoundException($"Document with ID {request.Id} not found.");

        document.Archive();

        await repository.UpdateAsync(document, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Document {Name} archived", document.Name);

        return new ArchiveDocumentResponse(
            document.Id,
            document.Name,
            document.Status);
    }
}
