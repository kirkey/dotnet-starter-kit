namespace FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Create.v1;

/// <summary>
/// Response from creating a new risk indicator.
/// </summary>
public sealed record CreateRiskIndicatorResponse(Guid Id, string Code, string Name);
