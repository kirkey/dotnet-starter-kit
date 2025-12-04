using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Get.v1;

public sealed record GetCollateralReleaseRequest(Guid Id) : IRequest<CollateralReleaseResponse>;
