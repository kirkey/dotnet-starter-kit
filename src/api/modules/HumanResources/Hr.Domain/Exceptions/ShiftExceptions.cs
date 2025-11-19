namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when shift is not found.
/// </summary>
public class ShiftNotFoundException(DefaultIdType id) : NotFoundException($"Shift with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when shift break is not found.
/// </summary>
public class ShiftBreakNotFoundException(DefaultIdType id)
    : NotFoundException($"Shift break with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when shift assignment is not found.
/// </summary>
public class ShiftAssignmentNotFoundException(DefaultIdType id)
    : NotFoundException($"Shift assignment with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when employee already has assignment for the period.
/// </summary>
public class ShiftAssignmentConflictException(DefaultIdType employeeId, DateTime startDate, DateTime endDate)
    : BadRequestException(
        $"Employee '{employeeId}' already has a shift assignment for {startDate:MMM d} to {endDate:MMM d, yyyy}.");

