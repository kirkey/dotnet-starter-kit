namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when timesheet is not found.
/// </summary>
public class TimesheetNotFoundException(DefaultIdType id)
    : NotFoundException($"Timesheet with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when timesheet already exists for period.
/// </summary>
public class TimesheetAlreadyExistsException(DefaultIdType employeeId, DateTime startDate, DateTime endDate)
    : BadRequestException(
        $"Timesheet already exists for employee '{employeeId}' for period {startDate:MMM d} to {endDate:MMM d, yyyy}.");

/// <summary>
/// Exception thrown when trying to modify locked timesheet.
/// </summary>
public class TimesheetLockedModificationException() : BadRequestException("Cannot modify locked timesheet.");

/// <summary>
/// Exception thrown when timesheet line not found.
/// </summary>
public class TimesheetLineNotFoundException(DefaultIdType id)
    : NotFoundException($"Timesheet line with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when invalid period type is provided.
/// </summary>
public class InvalidTimesheetPeriodTypeException(string periodType)
    : BadRequestException($"Period type '{periodType}' is not valid. Valid types are: Weekly, BiWeekly, Monthly.");

