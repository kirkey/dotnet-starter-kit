using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Deactivate.v1;

/// <summary>
/// Command to deactivate a risk indicator.
/// </summary>
public sealed record DeactivateRiskIndicatorCommand(Guid Id) : IRequest<DeactivateRiskIndicatorResponse>;
