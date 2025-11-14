namespace FSH.Starter.WebApi.HumanResources.Application.Attendance.Update.v1;

public sealed record UpdateAttendanceCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] TimeSpan? ClockInTime = null,
    [property: DefaultValue(null)] TimeSpan? ClockOutTime = null,
    [property: DefaultValue(null)] string? ClockInLocation = null,
    [property: DefaultValue(null)] string? ClockOutLocation = null,
    [property: DefaultValue(null)] string? Status = null,
    [property: DefaultValue(null)] string? Reason = null) : IRequest<UpdateAttendanceResponse>;

