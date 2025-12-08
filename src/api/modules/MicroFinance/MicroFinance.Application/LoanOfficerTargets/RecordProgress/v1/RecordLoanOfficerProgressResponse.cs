namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.RecordProgress.v1;

/// <summary>
/// Response from recording loan officer target progress.
/// </summary>
public sealed record RecordLoanOfficerProgressResponse(
    DefaultIdType Id,
    decimal AchievedValue,
    decimal AchievementPercentage,
    string Status,
    decimal EarnedIncentive);
