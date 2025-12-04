namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Create.v1;

/// <summary>
/// Response after creating a risk alert.
/// </summary>
public sealed record CreateRiskAlertResponse(
    Guid Id,
    string AlertNumber,
    string Title,
    string Severity,
    string Status);
