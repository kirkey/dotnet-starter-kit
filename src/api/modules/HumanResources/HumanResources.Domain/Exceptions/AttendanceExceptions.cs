namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when attendance record is not found.
/// </summary>
public class AttendanceNotFoundException : NotFoundException
{
    public AttendanceNotFoundException(DefaultIdType id)
        : base($"Attendance record with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when invalid attendance status is provided.
/// </summary>
public class InvalidAttendanceStatusException : BadRequestException
{
    public InvalidAttendanceStatusException(string status)
        : base($"Attendance status '{status}' is not valid. Valid statuses are: Present, Late, Absent, LeaveApproved, HalfDay.")
    {
    }
}

/// <summary>
/// Exception thrown when employee already has attendance for the date.
/// </summary>
public class AttendanceAlreadyExistsException : BadRequestException
{
    public AttendanceAlreadyExistsException(DefaultIdType employeeId, DateTime date)
        : base($"Attendance record already exists for employee '{employeeId}' on {date:MMM d, yyyy}.")
    {
    }
}

/// <summary>
/// Exception thrown when trying to clock out without clocking in.
/// </summary>
public class CannotClockOutWithoutClockInException : BadRequestException
{
    public CannotClockOutWithoutClockInException()
        : base("Cannot clock out without clocking in first.")
    {
    }
}

