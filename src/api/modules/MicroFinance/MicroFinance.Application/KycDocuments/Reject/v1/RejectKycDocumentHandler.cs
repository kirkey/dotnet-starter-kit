using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Reject.v1;

public sealed class RejectKycDocumentHandler(
    [FromKeyedServices("microfinance:kycdocuments")] IRepository<KycDocument> repository,
    ILogger<RejectKycDocumentHandler> logger)
    : IRequestHandler<RejectKycDocumentCommand, RejectKycDocumentResponse>
{
    public async Task<RejectKycDocumentResponse> Handle(
        RejectKycDocumentCommand request,
        CancellationToken cancellationToken)
    {
        var document = await repository.FirstOrDefaultAsync(
            new KycDocumentByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"KYC document {request.Id} not found");

        document.Reject(request.RejectedById, request.Reason);
        await repository.UpdateAsync(document, cancellationToken);

        logger.LogInformation("KYC document rejected: {DocumentId}", document.Id);

        return new RejectKycDocumentResponse(document.Id, document.Status, document.RejectionReason!);
    }
}
