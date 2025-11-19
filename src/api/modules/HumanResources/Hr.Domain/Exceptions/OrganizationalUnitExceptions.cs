namespace FSH.Starter.WebApi.HumanResources.Domain.Exceptions;

/// <summary>
/// Exception thrown when an organizational unit is not found.
/// </summary>
public class OrganizationalUnitNotFoundException : NotFoundException
{
    public OrganizationalUnitNotFoundException(DefaultIdType id)
        : base($"Organizational unit with ID '{id}' was not found.")
    {
    }

    public OrganizationalUnitNotFoundException(string code)
        : base($"Organizational unit with code '{code}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when an organizational unit code already exists.
/// </summary>
public class OrganizationalUnitCodeAlreadyExistsException(string code)
    : ConflictException($"Organizational unit with code '{code}' already exists.");

/// <summary>
/// Exception thrown when trying to create invalid hierarchy.
/// </summary>
public class InvalidOrganizationalHierarchyException(string message) : ConflictException(message);

