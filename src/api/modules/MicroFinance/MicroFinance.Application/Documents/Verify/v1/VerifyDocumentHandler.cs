using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.Verify.v1;

/// <summary>
/// Handler for verifying a document.
/// </summary>
public sealed class VerifyDocumentHandler(
    [FromKeyedServices("microfinance:documents")] IRepository<Document> repository,
    ILogger<VerifyDocumentHandler> logger)
    : IRequestHandler<VerifyDocumentCommand, VerifyDocumentResponse>
{
    public async Task<VerifyDocumentResponse> Handle(VerifyDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = await repository.GetByIdAsync(request.DocumentId, cancellationToken);

        if (document is null)
        {
            throw new NotFoundException($"Document with ID {request.DocumentId} not found.");
        }

        document.Verify(request.VerifiedById);
        await repository.UpdateAsync(document, cancellationToken);

        logger.LogInformation("Document {DocumentId} verified by {VerifiedById}",
            request.DocumentId, request.VerifiedById);

        return new VerifyDocumentResponse(
            document.Id,
            document.IsVerified,
            document.VerifiedAt!.Value);
    }
}
