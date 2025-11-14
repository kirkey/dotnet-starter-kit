namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Update.v1;

/// <summary>
/// Response for updating an attendance record.
/// </summary>
/// <param name="Id">The identifier of the updated attendance record.</param>
public sealed record UpdateAttendanceResponse(DefaultIdType Id);

