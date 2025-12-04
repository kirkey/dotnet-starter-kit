namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Assign.v1;

/// <summary>
/// Response after assigning a risk alert.
/// </summary>
public sealed record AssignRiskAlertResponse(Guid Id, Guid AssignedToUserId, string Status);
