namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when an employee education record is not found.
/// </summary>
public class EmployeeEducationNotFoundException : NotFoundException
{
    public EmployeeEducationNotFoundException(DefaultIdType id)
        : base($"Employee education record with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when invalid education level is provided.
/// </summary>
public class InvalidEducationLevelException : BadRequestException
{
    public InvalidEducationLevelException(string educationLevel)
        : base($"Education level '{educationLevel}' is not valid. Valid levels are: HighSchool, Associate, Bachelor, Master, Doctorate, Certification, Other.")
    {
    }
}

/// <summary>
/// Exception thrown when GPA is out of valid range.
/// </summary>
public class InvalidGpaException : BadRequestException
{
    public InvalidGpaException(decimal gpa)
        : base($"GPA '{gpa}' is not valid. GPA must be between 0.0 and 4.0.")
    {
    }
}

/// <summary>
/// Exception thrown when graduation date is in the future.
/// </summary>
public class InvalidGraduationDateException : BadRequestException
{
    public InvalidGraduationDateException(DateTime graduationDate)
        : base($"Graduation date '{graduationDate:MMM d, yyyy}' cannot be in the future.")
    {
    }
}

