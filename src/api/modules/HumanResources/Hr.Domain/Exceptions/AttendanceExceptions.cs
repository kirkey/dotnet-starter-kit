namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when attendance record is not found.
/// </summary>
public class AttendanceNotFoundException(DefaultIdType id)
    : NotFoundException($"Attendance record with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when duplicate attendance record exists for same date.
/// </summary>
public class DuplicateAttendanceException(DefaultIdType employeeId, DateTime date)
    : BadRequestException($"Attendance record already exists for employee '{employeeId}' on {date:yyyy-MM-dd}.");

/// <summary>
/// Exception thrown when clock out time is before clock in time.
/// </summary>
public class InvalidClockTimeException(TimeSpan clockIn, TimeSpan clockOut)
    : BadRequestException($"Clock out time ({clockOut}) cannot be before clock in time ({clockIn}).");

/// <summary>
/// Exception thrown when attempting invalid attendance status transition.
/// </summary>
public class InvalidAttendanceStatusException(string currentStatus, string attemptedStatus)
    : BadRequestException($"Cannot change status from '{currentStatus}' to '{attemptedStatus}'.");

