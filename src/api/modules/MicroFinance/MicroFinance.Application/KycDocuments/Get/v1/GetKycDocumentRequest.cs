using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Get.v1;

public sealed record GetKycDocumentRequest(DefaultIdType Id) : IRequest<KycDocumentResponse>;
