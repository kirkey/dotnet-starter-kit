namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.ClockOut.v1;

/// <summary>
/// Command to clock out an employee at end of shift.
/// Records clock out time and calculates hours worked.
/// </summary>
public sealed record ClockOutAttendanceCommand(
    DefaultIdType Id,
    [property: DefaultValue("17:00:00")] TimeSpan ClockOutTime,
    [property: DefaultValue(null)] string? ClockOutLocation = null,
    [property: DefaultValue(null)] string? Notes = null
) : IRequest<ClockOutAttendanceResponse>;

/// <summary>
/// Response for clock out operation.
/// </summary>
public sealed record ClockOutAttendanceResponse(
    DefaultIdType Id,
    TimeSpan? ClockOutTime,
    decimal HoursWorked);

