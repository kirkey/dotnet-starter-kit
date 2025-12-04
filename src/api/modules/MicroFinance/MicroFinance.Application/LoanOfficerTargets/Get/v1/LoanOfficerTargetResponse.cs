namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.Get.v1;

/// <summary>
/// Response containing loan officer target details.
/// </summary>
public sealed record LoanOfficerTargetResponse(
    Guid Id,
    Guid StaffId,
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
    decimal? IncentiveAmount,
    decimal? StretchBonus,
    decimal EarnedIncentive,
    string? Notes);
