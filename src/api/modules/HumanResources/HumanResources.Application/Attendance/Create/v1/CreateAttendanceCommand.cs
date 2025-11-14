namespace FSH.Starter.WebApi.HumanResources.Application.Attendance.Create.v1;

public sealed record CreateAttendanceCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType EmployeeId,
    [property: DefaultValue("2025-11-14")] DateTime AttendanceDate,
    [property: DefaultValue("09:00:00")] TimeSpan? ClockInTime = null,
    [property: DefaultValue("17:00:00")] TimeSpan? ClockOutTime = null,
    [property: DefaultValue(null)] string? ClockInLocation = null,
    [property: DefaultValue(null)] string? ClockOutLocation = null) : IRequest<CreateAttendanceResponse>;

