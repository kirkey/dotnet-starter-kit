using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Acknowledge.v1;

/// <summary>
/// Command to acknowledge a risk alert.
/// </summary>
public sealed record AcknowledgeRiskAlertCommand(DefaultIdType RiskAlertId, DefaultIdType UserId) : IRequest<AcknowledgeRiskAlertResponse>;
