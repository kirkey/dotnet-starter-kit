using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.Get.v1;

/// <summary>
/// Handler for getting a document by ID.
/// </summary>
public sealed class GetDocumentHandler(
    [FromKeyedServices("microfinance:documents")] IReadRepository<Document> repository,
    ILogger<GetDocumentHandler> logger)
    : IRequestHandler<GetDocumentRequest, DocumentResponse>
{
    public async Task<DocumentResponse> Handle(GetDocumentRequest request, CancellationToken cancellationToken)
    {
        var spec = new DocumentByIdSpec(request.Id);
        var document = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (document is null)
        {
            throw new NotFoundException($"Document with ID {request.Id} not found.");
        }

        logger.LogInformation("Retrieved document {DocumentId}", request.Id);

        return document;
    }
}
