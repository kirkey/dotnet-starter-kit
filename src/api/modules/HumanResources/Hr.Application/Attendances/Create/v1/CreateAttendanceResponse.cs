namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Create.v1;

/// <summary>
/// Response for creating an attendance record.
/// </summary>
/// <param name="Id">The identifier of the created attendance record.</param>
public sealed record CreateAttendanceResponse(DefaultIdType Id);

