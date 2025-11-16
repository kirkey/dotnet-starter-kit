namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when an employee education record is not found.
/// </summary>
public class EmployeeEducationNotFoundException(DefaultIdType id)
    : NotFoundException($"Employee education record with ID '{id}' was not found.");

/// <summary>
/// Exception thrown when invalid education level is provided.
/// </summary>
public class InvalidEducationLevelException(string educationLevel) : BadRequestException(
    $"Education level '{educationLevel}' is not valid. Valid levels are: HighSchool, Associate, Bachelor, Master, Doctorate, Certification, Other.");

/// <summary>
/// Exception thrown when GPA is out of valid range.
/// </summary>
public class InvalidGpaException(decimal gpa)
    : BadRequestException($"GPA '{gpa}' is not valid. GPA must be between 0.0 and 4.0.");

/// <summary>
/// Exception thrown when graduation date is in the future.
/// </summary>
public class InvalidGraduationDateException(DateTime graduationDate)
    : BadRequestException($"Graduation date '{graduationDate:MMM d, yyyy}' cannot be in the future.");

