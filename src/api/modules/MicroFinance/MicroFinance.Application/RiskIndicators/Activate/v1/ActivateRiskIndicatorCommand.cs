using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Activate.v1;

/// <summary>
/// Command to activate a risk indicator.
/// </summary>
public sealed record ActivateRiskIndicatorCommand(Guid Id) : IRequest<ActivateRiskIndicatorResponse>;
