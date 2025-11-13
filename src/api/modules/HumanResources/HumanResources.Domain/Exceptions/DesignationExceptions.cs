namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when a designation is not found.
/// </summary>
public class DesignationNotFoundException : NotFoundException
{
    public DesignationNotFoundException(DefaultIdType id)
        : base($"Designation with ID '{id}' was not found.")
    {
    }

    public DesignationNotFoundException(string code)
        : base($"Designation with code '{code}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when a designation code already exists in an organizational unit.
/// </summary>
public class DesignationCodeAlreadyExistsException : ConflictException
{
    public DesignationCodeAlreadyExistsException(string code)
        : base($"Designation with code '{code}' already exists in this organizational unit.")
    {
    }
}

