namespace FSH.Starter.WebApi.MicroFinance.Application.RiskIndicators.Get.v1;

/// <summary>
/// Response containing risk indicator details.
/// </summary>
public sealed record RiskIndicatorResponse(
    Guid Id,
    Guid RiskCategoryId,
    string Code,
    string Name,
    string? Description,
    string? Formula,
    string? Unit,
    string Direction,
    string Frequency,
    string? DataSource,
    decimal? TargetValue,
    decimal? GreenThreshold,
    decimal? YellowThreshold,
    decimal? OrangeThreshold,
    decimal? RedThreshold,
    decimal? CurrentValue,
    decimal? PreviousValue,
    string CurrentHealth,
    DateTime? LastMeasuredAt,
    decimal WeightFactor,
    string Status,
    string? Notes);
