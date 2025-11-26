using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents employee attendance record (clock in/out).
/// Tracks daily attendance, late arrivals, early departures, and absences.
/// </summary>
/// <remarks>
/// Key Design Points:
/// - Daily attendance tracking per employee
/// - Clock in/out times with location data
/// - Automatic calculation of worked hours
/// - Late/absent/leave status
/// - Manager approval workflow
/// 
/// Example:
/// - Employee John Doe on Nov 13, 2025
///   - Clock In: 8:00 AM (on time)
///   - Clock Out: 5:00 PM
///   - Worked: 9 hours
///   - Status: Present
/// </remarks>
public class Attendance : AuditableEntity, IAggregateRoot
{
    // Domain Constants - Binary Limits (Powers of 2)
    /// <summary>
    /// Maximum length for the clock in location field. (2^8 = 256)
    /// </summary>
    public const int ClockInLocationMaxLength = 256;

    /// <summary>
    /// Maximum length for the clock out location field. (2^8 = 256)
    /// </summary>
    public const int ClockOutLocationMaxLength = 256;

    /// <summary>
    /// Maximum length for the status field. (50)
    /// </summary>
    public const int StatusMaxLength = 50;

    /// <summary>
    /// Maximum length for the reason field. (2^9 = 512)
    /// </summary>
    public const int ReasonMaxLength = 512;

    /// <summary>
    /// Maximum length for the manager comment field. (2^9 = 512)
    /// </summary>
    public const int ManagerCommentMaxLength = 512;

    private Attendance() { }

    private Attendance(
        DefaultIdType id,
        DefaultIdType employeeId,
        DateTime attendanceDate,
        TimeSpan? clockInTime = null,
        TimeSpan? clockOutTime = null,
        string? clockInLocation = null,
        string? clockOutLocation = null)
    {
        Id = id;
        EmployeeId = employeeId;
        AttendanceDate = attendanceDate;
        ClockInTime = clockInTime;
        ClockOutTime = clockOutTime;
        ClockInLocation = clockInLocation;
        ClockOutLocation = clockOutLocation;
        Status = "Present";
        IsApproved = false;
        IsActive = true;

        QueueDomainEvent(new AttendanceCreated { Attendance = this });
    }

    /// <summary>
    /// The employee this attendance record is for.
    /// </summary>
    public DefaultIdType EmployeeId { get; private set; }
    public Employee Employee { get; private set; } = default!;

    /// <summary>
    /// Date of attendance.
    /// </summary>
    public DateTime AttendanceDate { get; private set; }

    /// <summary>
    /// Clock in time (HH:mm:ss format).
    /// </summary>
    public TimeSpan? ClockInTime { get; private set; }

    /// <summary>
    /// Clock out time (HH:mm:ss format).
    /// </summary>
    public TimeSpan? ClockOutTime { get; private set; }

    /// <summary>
    /// Location where employee clocked in.
    /// </summary>
    public string? ClockInLocation { get; private set; }

    /// <summary>
    /// Location where employee clocked out.
    /// </summary>
    public string? ClockOutLocation { get; private set; }

    /// <summary>
    /// Hours worked (calculated).
    /// </summary>
    public decimal HoursWorked { get; private set; }

    /// <summary>
    /// Status: Present, Late, Absent, LeaveApproved, etc.
    /// </summary>
    public string Status { get; private set; } = default!;

    /// <summary>
    /// Minutes late (if applicable).
    /// </summary>
    public int? MinutesLate { get; private set; }

    /// <summary>
    /// Reason for absence/leave (if applicable).
    /// </summary>
    public string? Reason { get; private set; }

    /// <summary>
    /// Whether attendance is approved by manager.
    /// </summary>
    public bool IsApproved { get; private set; }

    /// <summary>
    /// Manager's comments.
    /// </summary>
    public string? ManagerComment { get; private set; }

    /// <summary>
    /// Whether this record is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Creates a new attendance record.
    /// </summary>
    public static Attendance Create(
        DefaultIdType employeeId,
        DateTime attendanceDate,
        TimeSpan? clockInTime = null,
        TimeSpan? clockOutTime = null,
        string? clockInLocation = null,
        string? clockOutLocation = null)
    {
        var attendance = new Attendance(
            DefaultIdType.NewGuid(),
            employeeId,
            attendanceDate,
            clockInTime,
            clockOutTime,
            clockInLocation,
            clockOutLocation);

        return attendance;
    }

    /// <summary>
    /// Records clock in time.
    /// </summary>
    public Attendance ClockIn(TimeSpan time, string? location = null)
    {
        ClockInTime = time;
        ClockInLocation = location;
        QueueDomainEvent(new AttendanceClockInRecorded { Attendance = this });
        return this;
    }

    /// <summary>
    /// Records clock out time and calculates hours worked.
    /// </summary>
    public Attendance ClockOut(TimeSpan time, string? location = null)
    {
        ClockOutTime = time;
        ClockOutLocation = location;

        // Calculate hours worked
        if (ClockInTime.HasValue && ClockOutTime.HasValue)
        {
            var totalMinutes = (ClockOutTime.Value.TotalMinutes - ClockInTime.Value.TotalMinutes);
            if (totalMinutes < 0)
                totalMinutes += 24 * 60; // Handle overnight shifts

            HoursWorked = (decimal)(totalMinutes / 60.0);
        }

        QueueDomainEvent(new AttendanceClockOutRecorded { Attendance = this });
        return this;
    }

    /// <summary>
    /// Marks as late with minutes late.
    /// </summary>
    public Attendance MarkAsLate(int minutesLate, string? reason = null)
    {
        Status = "Late";
        MinutesLate = minutesLate;
        Reason = reason;
        QueueDomainEvent(new AttendanceMarkedAsLate { Attendance = this });
        return this;
    }

    /// <summary>
    /// Marks as absent.
    /// </summary>
    public Attendance MarkAsAbsent(string? reason = null)
    {
        Status = "Absent";
        Reason = reason;
        QueueDomainEvent(new AttendanceMarkedAsAbsent { Attendance = this });
        return this;
    }

    /// <summary>
    /// Marks as leave approved.
    /// </summary>
    public Attendance MarkAsLeave(string? reason = null)
    {
        Status = "LeaveApproved";
        Reason = reason;
        QueueDomainEvent(new AttendanceMarkedAsLeave { Attendance = this });
        return this;
    }

    /// <summary>
    /// Approves attendance.
    /// </summary>
    public Attendance Approve(string? comment = null)
    {
        IsApproved = true;
        ManagerComment = comment;
        QueueDomainEvent(new AttendanceApproved { AttendanceId = Id, ManagerComment = comment });
        return this;
    }

    /// <summary>
    /// Rejects attendance.
    /// </summary>
    public Attendance Reject(string? comment = null)
    {
        IsApproved = false;
        ManagerComment = comment;
        QueueDomainEvent(new AttendanceRejected { AttendanceId = Id, ManagerComment = comment });
        return this;
    }

    /// <summary>
    /// Deactivates this attendance record.
    /// </summary>
    public Attendance Deactivate()
    {
        IsActive = false;
        QueueDomainEvent(new AttendanceDeactivated { AttendanceId = Id });
        return this;
    }

    /// <summary>
    /// Activates this attendance record.
    /// </summary>
    public Attendance Activate()
    {
        IsActive = true;
        QueueDomainEvent(new AttendanceActivated { AttendanceId = Id });
        return this;
    }
}

/// <summary>
/// Attendance status constants.
/// </summary>
public static class AttendanceStatus
{
    public const string Present = "Present";
    public const string Late = "Late";
    public const string Absent = "Absent";
    public const string LeaveApproved = "LeaveApproved";
    public const string HalfDay = "HalfDay";
}

