namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Start.v1;

/// <summary>
/// Response from starting a staff training.
/// </summary>
public sealed record StartStaffTrainingResponse(DefaultIdType Id, string Status);
