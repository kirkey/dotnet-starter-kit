using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Get.v1;

public sealed record GetKycDocumentRequest(Guid Id) : IRequest<KycDocumentResponse>;
