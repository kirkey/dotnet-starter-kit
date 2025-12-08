using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.MarkExpired.v1;

/// <summary>
/// Handler for marking a document as expired.
/// </summary>
public sealed class MarkExpiredDocumentHandler(
    ILogger<MarkExpiredDocumentHandler> logger,
    [FromKeyedServices("microfinance:documents")] IRepository<Document> repository)
    : IRequestHandler<MarkExpiredDocumentCommand, MarkExpiredDocumentResponse>
{
    /// <summary>
    /// Handles the mark expired document command.
    /// </summary>
    public async Task<MarkExpiredDocumentResponse> Handle(
        MarkExpiredDocumentCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var document = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new KeyNotFoundException($"Document with ID {request.Id} not found.");

        document.MarkExpired();

        await repository.UpdateAsync(document, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Document {Name} marked as expired", document.Name);

        return new MarkExpiredDocumentResponse(
            document.Id,
            document.Name,
            document.Status);
    }
}
