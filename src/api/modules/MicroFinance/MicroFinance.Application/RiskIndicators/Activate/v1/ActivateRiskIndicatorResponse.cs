namespace FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Activate.v1;

/// <summary>
/// Response from activating a risk indicator.
/// </summary>
public sealed record ActivateRiskIndicatorResponse(Guid Id, string Status);
