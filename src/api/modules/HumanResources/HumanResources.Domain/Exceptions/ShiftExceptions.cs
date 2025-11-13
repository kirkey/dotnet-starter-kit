namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when shift is not found.
/// </summary>
public class ShiftNotFoundException : NotFoundException
{
    public ShiftNotFoundException(DefaultIdType id)
        : base($"Shift with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when shift break is not found.
/// </summary>
public class ShiftBreakNotFoundException : NotFoundException
{
    public ShiftBreakNotFoundException(DefaultIdType id)
        : base($"Shift break with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when shift assignment is not found.
/// </summary>
public class ShiftAssignmentNotFoundException : NotFoundException
{
    public ShiftAssignmentNotFoundException(DefaultIdType id)
        : base($"Shift assignment with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when employee already has assignment for the period.
/// </summary>
public class ShiftAssignmentConflictException : BadRequestException
{
    public ShiftAssignmentConflictException(DefaultIdType employeeId, DateTime startDate, DateTime endDate)
        : base($"Employee '{employeeId}' already has a shift assignment for {startDate:MMM d} to {endDate:MMM d, yyyy}.")
    {
    }
}

