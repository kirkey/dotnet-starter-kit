namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when an employee is not found.
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
public class EmployeeNumberAlreadyExistsException : ConflictException
{
    public EmployeeNumberAlreadyExistsException(string employeeNumber)
        : base($"Employee number '{employeeNumber}' already exists.")
    {
    }
}

/// <summary>
/// Exception thrown when trying to hire an already active employee.
/// </summary>
public class EmployeeAlreadyHiredException : ConflictException
{
    public EmployeeAlreadyHiredException(DefaultIdType employeeId)
        : base($"Employee '{employeeId}' is already hired.")
    {
    }
}

/// <summary>
/// Exception thrown when trying to operate on a terminated employee.
/// </summary>
public class TerminatedEmployeeException : ConflictException
{
    public TerminatedEmployeeException(DefaultIdType employeeId)
        : base($"Cannot perform operation on terminated employee '{employeeId}'.")
    {
    }
}

/// <summary>
/// Exception thrown when employee does not have a current designation.
/// </summary>
public class NoCurrentDesignationException : NotFoundException
{
    public NoCurrentDesignationException(DefaultIdType employeeId)
        : base($"Employee '{employeeId}' does not have a current primary designation.")
    {
    }
}

