using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Get.v1;

public sealed record GetCollateralValuationRequest(DefaultIdType Id) : IRequest<CollateralValuationResponse>;
