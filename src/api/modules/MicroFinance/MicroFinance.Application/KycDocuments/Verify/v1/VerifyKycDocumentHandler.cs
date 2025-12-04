using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Verify.v1;

public sealed class VerifyKycDocumentHandler(
    [FromKeyedServices("microfinance:kycdocuments")] IRepository<KycDocument> repository,
    ILogger<VerifyKycDocumentHandler> logger)
    : IRequestHandler<VerifyKycDocumentCommand, VerifyKycDocumentResponse>
{
    public async Task<VerifyKycDocumentResponse> Handle(
        VerifyKycDocumentCommand request,
        CancellationToken cancellationToken)
    {
        var document = await repository.FirstOrDefaultAsync(
            new KycDocumentByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"KYC document {request.Id} not found");

        document.Verify(request.VerifiedById, request.Notes);
        await repository.UpdateAsync(document, cancellationToken);

        logger.LogInformation("KYC document verified: {DocumentId}", document.Id);

        return new VerifyKycDocumentResponse(document.Id, document.Status, document.VerifiedAt!.Value);
    }
}
