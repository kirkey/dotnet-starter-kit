namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when employee is not found.
/// </summary>
public class EmployeeNotFoundException : NotFoundException
{
    public EmployeeNotFoundException(DefaultIdType id)
        : base($"Employee with ID '{id}' was not found.")
    {
    }

    public EmployeeNotFoundException(string employeeNumber)
        : base($"Employee with number '{employeeNumber}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when employee number already exists.
/// </summary>
public class DuplicateEmployeeNumberException(string employeeNumber)
    : BadRequestException($"Employee number '{employeeNumber}' already exists.");

/// <summary>
/// Exception thrown when employee cannot be terminated (e.g., already terminated).
/// </summary>
public class InvalidEmployeeTerminationException(DefaultIdType employeeId, string currentStatus)
    : BadRequestException($"Cannot terminate employee '{employeeId}' with status '{currentStatus}'.");

/// <summary>
/// Exception thrown when invalid employment status is provided.
/// </summary>
public class InvalidEmploymentStatusException(string status)
    : BadRequestException($"Employment status '{status}' is not valid.");

