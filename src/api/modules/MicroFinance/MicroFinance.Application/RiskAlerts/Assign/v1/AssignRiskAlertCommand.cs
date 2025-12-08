using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Assign.v1;

/// <summary>
/// Command to assign a risk alert for investigation.
/// </summary>
public sealed record AssignRiskAlertCommand(DefaultIdType RiskAlertId, DefaultIdType UserId) : IRequest<AssignRiskAlertResponse>;
