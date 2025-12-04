using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Escalate.v1;

/// <summary>
/// Command to escalate a risk alert.
/// </summary>
public sealed record EscalateRiskAlertCommand(Guid RiskAlertId) : IRequest<EscalateRiskAlertResponse>;
