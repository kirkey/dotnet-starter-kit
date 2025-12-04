using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Get.v1;

/// <summary>
/// Request to get a risk indicator by ID.
/// </summary>
public sealed record GetRiskIndicatorRequest(Guid Id) : IRequest<RiskIndicatorResponse>;
