namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Acknowledge.v1;

/// <summary>
/// Response after acknowledging a risk alert.
/// </summary>
public sealed record AcknowledgeRiskAlertResponse(DefaultIdType Id, string Status, DateTime AcknowledgedAt);
