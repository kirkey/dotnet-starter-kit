namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when an employee designation assignment is not found.
/// </summary>
public class DesignationAssignmentNotFoundException(DefaultIdType id)
    : NotFoundException($"Employee designation assignment with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when trying to assign multiple plantilla designations to same employee.
/// </summary>
public class MultipleActivePlantillaException(DefaultIdType employeeId) : ConflictException(
    $"Employee '{employeeId}' already has an active plantilla designation. End the current one before assigning a new plantilla.");

/// <summary>
/// Exception thrown when designation assignment dates are invalid.
/// </summary>
public class InvalidDesignationAssignmentDatesException(string message) : BadRequestException(message);

/// <summary>
/// Exception thrown when trying to assign same designation to employee multiple times simultaneously.
/// </summary>
public class DuplicateDesignationAssignmentException(DefaultIdType employeeId, DefaultIdType designationId)
    : ConflictException($"Employee '{employeeId}' already has an active assignment for designation '{designationId}'.");

