namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Update.v1;

/// <summary>
/// Command to update an attendance record (clock out or mark status).
/// </summary>
public sealed record UpdateAttendanceCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue("17:00:00")] TimeSpan? ClockOutTime = null,
    [property: DefaultValue(null)] string? ClockOutLocation = null,
    [property: DefaultValue(null)] string? Status = null,
    [property: DefaultValue(null)] int? MinutesLate = null,
    [property: DefaultValue(null)] string? Reason = null,
    [property: DefaultValue(null)] string? ManagerComment = null) : IRequest<UpdateAttendanceResponse>;

