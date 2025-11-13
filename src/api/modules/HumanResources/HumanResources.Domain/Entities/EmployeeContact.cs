using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents emergency contacts and personal references for an employee.
/// Tracks contact information for family, emergency contacts, and professional references.
/// </summary>
/// <remarks>
/// Key Design Points:
/// - Multiple contacts per employee (emergency, family, reference)
/// - Contact type determines usage (emergency, next-of-kin, professional reference)
/// - Includes relationship information
/// - Supports phone, email, and address
/// - Priority/order for emergency contacts
/// 
/// Example:
/// - Employee John Doe has:
///   - Emergency Contact: Wife (Jane Doe)
///   - Next of Kin: Mother (Mary Doe)
///   - Reference: Former Manager (Bob Smith)
/// </remarks>
public class EmployeeContact : AuditableEntity, IAggregateRoot
{
    private EmployeeContact() { }

    private EmployeeContact(
        DefaultIdType id,
        DefaultIdType employeeId,
        string firstName,
        string lastName,
        string contactType,
        string? relationship = null,
        string? phoneNumber = null,
        string? email = null,
        string? address = null)
    {
        Id = id;
        EmployeeId = employeeId;
        FirstName = firstName;
        LastName = lastName;
        ContactType = contactType;
        Relationship = relationship;
        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
        IsActive = true;
        Priority = 1;

        QueueDomainEvent(new EmployeeContactCreated { Contact = this });
    }

    /// <summary>
    /// The employee this contact is associated with.
    /// </summary>
    public DefaultIdType EmployeeId { get; private set; }
    public Employee Employee { get; private set; } = default!;

    /// <summary>
    /// First name of the contact.
    /// </summary>
    public string FirstName { get; private set; } = default!;

    /// <summary>
    /// Last name of the contact.
    /// </summary>
    public string LastName { get; private set; } = default!;

    /// <summary>
    /// Full name of the contact (read-only, computed).
    /// </summary>
    public string FullName => $"{FirstName} {LastName}".Trim();

    /// <summary>
    /// Type of contact (Emergency, NextOfKin, Reference, Family).
    /// </summary>
    public string ContactType { get; private set; } = default!;

    /// <summary>
    /// Relationship to employee (Spouse, Parent, Sibling, etc.).
    /// </summary>
    public string? Relationship { get; private set; }

    /// <summary>
    /// Contact phone number.
    /// </summary>
    public string? PhoneNumber { get; private set; }

    /// <summary>
    /// Contact email address.
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Contact address.
    /// </summary>
    public string? Address { get; private set; }

    /// <summary>
    /// Priority order for emergency contacts (1 = first contact).
    /// </summary>
    public int Priority { get; private set; }

    /// <summary>
    /// Whether this contact is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Creates a new employee contact.
    /// </summary>
    public static EmployeeContact Create(
        DefaultIdType employeeId,
        string firstName,
        string lastName,
        string contactType,
        string? relationship = null,
        string? phoneNumber = null,
        string? email = null,
        string? address = null)
    {
        var contact = new EmployeeContact(
            DefaultIdType.NewGuid(),
            employeeId,
            firstName,
            lastName,
            contactType,
            relationship,
            phoneNumber,
            email,
            address);

        return contact;
    }

    /// <summary>
    /// Updates contact information.
    /// </summary>
    public EmployeeContact Update(
        string? firstName = null,
        string? lastName = null,
        string? relationship = null,
        string? phoneNumber = null,
        string? email = null,
        string? address = null)
    {
        if (!string.IsNullOrWhiteSpace(firstName))
            FirstName = firstName;

        if (!string.IsNullOrWhiteSpace(lastName))
            LastName = lastName;

        if (relationship != null)
            Relationship = relationship;

        if (phoneNumber != null)
            PhoneNumber = phoneNumber;

        if (email != null)
            Email = email;

        if (address != null)
            Address = address;

        QueueDomainEvent(new EmployeeContactUpdated { Contact = this });
        return this;
    }

    /// <summary>
    /// Sets the priority order for this contact.
    /// </summary>
    public EmployeeContact SetPriority(int priority)
    {
        if (priority < 1)
            throw new ArgumentException("Priority must be at least 1.", nameof(priority));

        Priority = priority;
        QueueDomainEvent(new EmployeeContactUpdated { Contact = this });
        return this;
    }

    /// <summary>
    /// Deactivates this contact.
    /// </summary>
    public EmployeeContact Deactivate()
    {
        IsActive = false;
        QueueDomainEvent(new EmployeeContactDeactivated { ContactId = Id });
        return this;
    }

    /// <summary>
    /// Activates this contact.
    /// </summary>
    public EmployeeContact Activate()
    {
        IsActive = true;
        QueueDomainEvent(new EmployeeContactActivated { ContactId = Id });
        return this;
    }
}

/// <summary>
/// Contact type constants.
/// </summary>
public static class EmployeeContactType
{
    public const string Emergency = "Emergency";
    public const string NextOfKin = "NextOfKin";
    public const string Reference = "Reference";
    public const string Family = "Family";
}

