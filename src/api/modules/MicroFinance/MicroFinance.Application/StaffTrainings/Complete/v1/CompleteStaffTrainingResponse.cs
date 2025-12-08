namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Complete.v1;

/// <summary>
/// Response from completing a staff training.
/// </summary>
public sealed record CompleteStaffTrainingResponse(
    DefaultIdType Id,
    string Status,
    decimal? Score,
    DateOnly? CompletionDate);
