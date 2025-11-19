using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

/// <summary>
/// Event raised when attendance record is created.
/// </summary>
public record AttendanceCreated : DomainEvent
{
    public Attendance Attendance { get; init; } = default!;
}

/// <summary>
/// Event raised when employee clocks in.
/// </summary>
public record AttendanceClockInRecorded : DomainEvent
{
    public Attendance Attendance { get; init; } = default!;
}

/// <summary>
/// Event raised when employee clocks out.
/// </summary>
public record AttendanceClockOutRecorded : DomainEvent
{
    public Attendance Attendance { get; init; } = default!;
}

/// <summary>
/// Event raised when attendance is marked as late.
/// </summary>
public record AttendanceMarkedAsLate : DomainEvent
{
    public Attendance Attendance { get; init; } = default!;
}

/// <summary>
/// Event raised when attendance is marked as absent.
/// </summary>
public record AttendanceMarkedAsAbsent : DomainEvent
{
    public Attendance Attendance { get; init; } = default!;
}

/// <summary>
/// Event raised when attendance is marked as leave approved.
/// </summary>
public record AttendanceMarkedAsLeave : DomainEvent
{
    public Attendance Attendance { get; init; } = default!;
}

/// <summary>
/// Event raised when attendance is approved.
/// </summary>
public record AttendanceApproved : DomainEvent
{
    public DefaultIdType AttendanceId { get; init; }
    public string? ManagerComment { get; init; }
}

/// <summary>
/// Event raised when attendance is rejected.
/// </summary>
public record AttendanceRejected : DomainEvent
{
    public DefaultIdType AttendanceId { get; init; }
    public string? ManagerComment { get; init; }
}

/// <summary>
/// Event raised when attendance is deactivated.
/// </summary>
public record AttendanceDeactivated : DomainEvent
{
    public DefaultIdType AttendanceId { get; init; }
}

/// <summary>
/// Event raised when attendance is activated.
/// </summary>
public record AttendanceActivated : DomainEvent
{
    public DefaultIdType AttendanceId { get; init; }
}


