using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Get.v1;

public sealed record GetCollateralReleaseRequest(DefaultIdType Id) : IRequest<CollateralReleaseResponse>;
