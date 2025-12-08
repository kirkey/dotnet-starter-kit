namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Escalate.v1;

/// <summary>
/// Response after escalating a risk alert.
/// </summary>
public sealed record EscalateRiskAlertResponse(DefaultIdType Id, int EscalationLevel, string Severity);
