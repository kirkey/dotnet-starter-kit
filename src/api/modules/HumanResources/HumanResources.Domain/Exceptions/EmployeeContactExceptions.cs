namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when an employee contact is not found.
/// </summary>
public class EmployeeContactNotFoundException : NotFoundException
{
    public EmployeeContactNotFoundException(DefaultIdType id)
        : base($"Employee contact with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when invalid contact type is provided.
/// </summary>
public class InvalidEmployeeContactTypeException : BadRequestException
{
    public InvalidEmployeeContactTypeException(string contactType)
        : base($"Contact type '{contactType}' is not valid. Valid types are: Emergency, NextOfKin, Reference, Family.")
    {
    }
}

/// <summary>
/// Exception thrown when no emergency contacts are found for an employee.
/// </summary>
public class NoEmergencyContactsException : NotFoundException
{
    public NoEmergencyContactsException(DefaultIdType employeeId)
        : base($"No emergency contacts found for employee '{employeeId}'.")
    {
    }
}

