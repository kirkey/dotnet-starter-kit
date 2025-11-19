namespace FSH.Starter.WebApi.HumanResources.Application.Attendances.Create.v1;

/// <summary>
/// Command to create a new attendance record (clock in).
/// </summary>
public sealed record CreateAttendanceCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType EmployeeId,
    [property: DefaultValue("08:00:00")] TimeSpan ClockInTime,
    [property: DefaultValue(null)] string? ClockInLocation = null) : IRequest<CreateAttendanceResponse>;

