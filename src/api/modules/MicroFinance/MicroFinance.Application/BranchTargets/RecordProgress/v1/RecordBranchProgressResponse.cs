namespace FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.RecordProgress.v1;

/// <summary>
/// Response from recording branch target progress.
/// </summary>
public sealed record RecordBranchProgressResponse(
    DefaultIdType Id,
    decimal AchievedValue,
    decimal AchievementPercentage,
    string Status);
