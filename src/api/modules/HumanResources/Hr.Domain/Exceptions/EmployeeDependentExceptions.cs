namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when an employee dependent is not found.
/// </summary>
public class EmployeeDependentNotFoundException(DefaultIdType id)
    : NotFoundException($"Employee dependent with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when invalid dependent type is provided.
/// </summary>
public class InvalidDependentTypeException(string dependentType) : BadRequestException(
    $"Dependent type '{dependentType}' is not valid. Valid types are: Spouse, Child, Parent, Sibling, Other.");

/// <summary>
/// Exception thrown when dependent age exceeds maximum allowed age.
/// </summary>
public class DependentAgeExceededException(int age, int maxAge)
    : BadRequestException($"Dependent age {age} exceeds maximum allowed age of {maxAge}.");

