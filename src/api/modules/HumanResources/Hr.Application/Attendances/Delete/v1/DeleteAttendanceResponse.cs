namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Delete.v1;

/// <summary>
/// Response for deleting an attendance record.
/// </summary>
/// <param name="Id">The identifier of the deleted attendance record.</param>
public sealed record DeleteAttendanceResponse(DefaultIdType Id);

