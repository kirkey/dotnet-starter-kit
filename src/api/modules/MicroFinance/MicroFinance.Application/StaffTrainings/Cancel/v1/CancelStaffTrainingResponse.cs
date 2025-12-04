namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Cancel.v1;

/// <summary>
/// Response from cancelling a staff training.
/// </summary>
public sealed record CancelStaffTrainingResponse(Guid Id, string Status);
