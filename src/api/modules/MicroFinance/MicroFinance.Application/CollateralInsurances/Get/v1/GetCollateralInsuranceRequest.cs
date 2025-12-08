using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.Get.v1;

public sealed record GetCollateralInsuranceRequest(DefaultIdType Id) : IRequest<CollateralInsuranceResponse>;
