namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Resolve.v1;

/// <summary>
/// Response after resolving a risk alert.
/// </summary>
public sealed record ResolveRiskAlertResponse(Guid Id, string Status, DateTime ResolvedAt);
