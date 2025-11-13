namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when an employee dependent is not found.
/// </summary>
public class EmployeeDependentNotFoundException : NotFoundException
{
    public EmployeeDependentNotFoundException(DefaultIdType id)
        : base($"Employee dependent with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when invalid dependent type is provided.
/// </summary>
public class InvalidDependentTypeException : BadRequestException
{
    public InvalidDependentTypeException(string dependentType)
        : base($"Dependent type '{dependentType}' is not valid. Valid types are: Spouse, Child, Parent, Sibling, Other.")
    {
    }
}

/// <summary>
/// Exception thrown when dependent age exceeds maximum allowed age.
/// </summary>
public class DependentAgeExceededException : BadRequestException
{
    public DependentAgeExceededException(int age, int maxAge)
        : base($"Dependent age {age} exceeds maximum allowed age of {maxAge}.")
    {
    }
}

