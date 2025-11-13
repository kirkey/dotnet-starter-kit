namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when an employee designation assignment is not found.
/// </summary>
public class EmployeeDesignationAssignmentNotFoundException : NotFoundException
{
    public EmployeeDesignationAssignmentNotFoundException(DefaultIdType id)
        : base($"Employee designation assignment with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when trying to assign multiple plantilla designations to same employee.
/// </summary>
public class MultipleActivePlantillaException : ConflictException
{
    public MultipleActivePlantillaException(DefaultIdType employeeId)
        : base($"Employee '{employeeId}' already has an active plantilla designation. End the current one before assigning a new plantilla.")
    {
    }
}

/// <summary>
/// Exception thrown when designation assignment dates are invalid.
/// </summary>
public class InvalidDesignationAssignmentDatesException : BadRequestException
{
    public InvalidDesignationAssignmentDatesException(string message)
        : base(message)
    {
    }
}

/// <summary>
/// Exception thrown when trying to assign same designation to employee multiple times simultaneously.
/// </summary>
public class DuplicateDesignationAssignmentException : ConflictException
{
    public DuplicateDesignationAssignmentException(DefaultIdType employeeId, DefaultIdType designationId)
        : base($"Employee '{employeeId}' already has an active assignment for designation '{designationId}'.")
    {
    }
}

