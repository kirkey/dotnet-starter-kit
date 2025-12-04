using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Get.v1;

/// <summary>
/// Request to get a collateral type by ID.
/// </summary>
public sealed record GetCollateralTypeRequest(Guid Id) : IRequest<CollateralTypeResponse>;
