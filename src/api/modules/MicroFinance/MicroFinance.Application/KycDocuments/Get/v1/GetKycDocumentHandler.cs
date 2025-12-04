using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Get.v1;

public sealed class GetKycDocumentHandler(
    [FromKeyedServices("microfinance:kycdocuments")] IReadRepository<KycDocument> repository)
    : IRequestHandler<GetKycDocumentRequest, KycDocumentResponse>
{
    public async Task<KycDocumentResponse> Handle(
        GetKycDocumentRequest request,
        CancellationToken cancellationToken)
    {
        var document = await repository.FirstOrDefaultAsync(
            new KycDocumentByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"KYC document {request.Id} not found");

        return new KycDocumentResponse(
            document.Id,
            document.MemberId,
            document.DocumentType,
            document.DocumentNumber,
            document.FileName,
            document.FilePath,
            document.MimeType,
            document.FileSize,
            document.IssueDate,
            document.ExpiryDate,
            document.IssuingAuthority,
            document.Status,
            document.VerifiedAt,
            document.VerifiedById,
            document.RejectionReason,
            document.IsPrimary);
    }
}
