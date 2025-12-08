using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Get.v1;

public sealed record GetShareProductRequest(DefaultIdType Id) : IRequest<ShareProductResponse>;
