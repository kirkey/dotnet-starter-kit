namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Get.v1;

/// <summary>
/// Response object for EmployeeContact entity details.
/// </summary>
public sealed record EmployeeContactResponse
{
    /// <summary>
    /// Gets the unique identifier of the contact.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Gets the employee identifier associated with this contact.
    /// </summary>
    public DefaultIdType EmployeeId { get; init; }

    /// <summary>
    /// Gets the first name of the contact.
    /// </summary>
    public string FirstName { get; init; } = default!;

    /// <summary>
    /// Gets the last name of the contact.
    /// </summary>
    public string LastName { get; init; } = default!;

    /// <summary>
    /// Gets the full name of the contact.
    /// </summary>
    public string FullName { get; init; } = default!;

    /// <summary>
    /// Gets the contact type (Emergency, NextOfKin, Reference, Family).
    /// </summary>
    public string ContactType { get; init; } = default!;

    /// <summary>
    /// Gets the relationship to the employee.
    /// </summary>
    public string? Relationship { get; init; }

    /// <summary>
    /// Gets the phone number of the contact.
    /// </summary>
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// Gets the email address of the contact.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Gets the address of the contact.
    /// </summary>
    public string? Address { get; init; }

    /// <summary>
    /// Gets the priority order for emergency contacts.
    /// </summary>
    public int Priority { get; init; }

    /// <summary>
    /// Gets a value indicating whether the contact is active.
    /// </summary>
    public bool IsActive { get; init; }
}

