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
/// Exception thrown when duplicate attendance record exists for same date.
/// </summary>
public class DuplicateAttendanceException : BadRequestException
{
    public DuplicateAttendanceException(DefaultIdType employeeId, DateTime date)
        : base($"Attendance record already exists for employee '{employeeId}' on {date:yyyy-MM-dd}.")
    {
    }
}

/// <summary>
/// Exception thrown when clock out time is before clock in time.
/// </summary>
public class InvalidClockTimeException : BadRequestException
{
    public InvalidClockTimeException(TimeSpan clockIn, TimeSpan clockOut)
        : base($"Clock out time ({clockOut}) cannot be before clock in time ({clockIn}).")
    {
    }
}

/// <summary>
/// Exception thrown when attempting invalid attendance status transition.
/// </summary>
public class InvalidAttendanceStatusException : BadRequestException
{
    public InvalidAttendanceStatusException(string currentStatus, string attemptedStatus)
        : base($"Cannot change status from '{currentStatus}' to '{attemptedStatus}'.")
    {
    }
}

