namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Schedule.v1;

/// <summary>
/// Response from scheduling a staff training.
/// </summary>
public sealed record ScheduleStaffTrainingResponse(Guid Id, string TrainingName, DateOnly StartDate, string Status);
