using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Create.v1;

public sealed class CreateKycDocumentHandler(
    [FromKeyedServices("microfinance:kycdocuments")] IRepository<KycDocument> repository,
    ILogger<CreateKycDocumentHandler> logger)
    : IRequestHandler<CreateKycDocumentCommand, CreateKycDocumentResponse>
{
    public async Task<CreateKycDocumentResponse> Handle(
        CreateKycDocumentCommand request,
        CancellationToken cancellationToken)
    {
        var document = KycDocument.Create(
            request.MemberId,
            request.DocumentType,
            request.FileName,
            request.FilePath,
            request.MimeType,
            request.FileSize);

        if (request.DocumentNumber != null || request.IssueDate.HasValue || 
            request.ExpiryDate.HasValue || request.IssuingAuthority != null)
        {
            document.WithDocumentDetails(
                request.DocumentNumber,
                request.IssueDate,
                request.ExpiryDate,
                request.IssuingAuthority);
        }

        await repository.AddAsync(document, cancellationToken);

        logger.LogInformation("KYC document created: {DocumentId} for member {MemberId}",
            document.Id, request.MemberId);

        return new CreateKycDocumentResponse(document.Id);
    }
}
