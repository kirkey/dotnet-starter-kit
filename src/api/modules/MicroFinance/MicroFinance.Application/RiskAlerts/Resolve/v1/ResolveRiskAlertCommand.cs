using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Resolve.v1;

/// <summary>
/// Command to resolve a risk alert.
/// </summary>
public sealed record ResolveRiskAlertCommand(DefaultIdType RiskAlertId, DefaultIdType UserId, string Resolution) : IRequest<ResolveRiskAlertResponse>;
