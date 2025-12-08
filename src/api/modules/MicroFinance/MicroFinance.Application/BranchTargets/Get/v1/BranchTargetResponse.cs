namespace FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.Get.v1;

/// <summary>
/// Response containing branch target details.
/// </summary>
public sealed record BranchTargetResponse(
    DefaultIdType Id,
    DefaultIdType BranchId,
    string TargetType,
    string? Description,
    string Period,
    DateOnly PeriodStart,
    DateOnly PeriodEnd,
    decimal TargetValue,
    string? MetricUnit,
    decimal AchievedValue,
    decimal AchievementPercentage,
    string Status,
    decimal? MinimumThreshold,
    decimal? StretchTarget,
    decimal Weight,
    string? Notes);
