using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Create.v1;

/// <summary>
/// Command to create a new risk indicator.
/// </summary>
public sealed record CreateRiskIndicatorCommand(
    DefaultIdType RiskCategoryId,
    string Code,
    string Name,
    string Direction,
    string Frequency,
    string? Description = null,
    string? Formula = null,
    string? Unit = null,
    string? DataSource = null,
    decimal? TargetValue = null,
    decimal? GreenThreshold = null,
    decimal? YellowThreshold = null,
    decimal? OrangeThreshold = null,
    decimal? RedThreshold = null,
    decimal WeightFactor = 1.0m) : IRequest<CreateRiskIndicatorResponse>;
