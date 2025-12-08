namespace FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Deactivate.v1;

/// <summary>
/// Response from deactivating a risk indicator.
/// </summary>
public sealed record DeactivateRiskIndicatorResponse(DefaultIdType Id, string Status);
