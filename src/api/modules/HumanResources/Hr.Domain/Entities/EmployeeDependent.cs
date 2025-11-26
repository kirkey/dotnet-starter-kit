using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Represents an employee's dependent (family member).
/// Tracks dependent information for tax exemptions, benefits, and emergency purposes.
/// </summary>
/// <remarks>
/// Key Design Points:
/// - Multiple dependents per employee
/// - Dependent type: Spouse, Child, Parent, Sibling, Other
/// - Date of birth for age calculation
/// - Relationship type affects tax and benefit eligibility
/// - Supports tax exemptions and benefit coverage
/// 
/// Example:
/// - Employee John Doe has:
///   - Spouse: Jane Doe (DOB: 1990-05-15)
///   - Child 1: Jack Doe (DOB: 2015-03-20)
///   - Child 2: Jill Doe (DOB: 2017-08-10)
/// </remarks>
public class EmployeeDependent : AuditableEntity, IAggregateRoot
{
    private EmployeeDependent() { }

    private EmployeeDependent(
        DefaultIdType id,
        DefaultIdType employeeId,
        string firstName,
        string lastName,
        string dependentType,
        DateTime? dateOfBirth,
        string? relationship = null,
        string? ssn = null,
        string? email = null,
        string? phoneNumber = null)
    {
        Id = id;
        EmployeeId = employeeId;
        FirstName = firstName;
        LastName = lastName;
        DependentType = dependentType;
        DateOfBirth = dateOfBirth;
        Relationship = relationship;
        Ssn = ssn;
        Email = email;
        PhoneNumber = phoneNumber;
        IsActive = true;
        IsBeneficiary = false;
        IsClaimableDependent = true;

        QueueDomainEvent(new EmployeeDependentCreated { Dependent = this });
    }

    /// <summary>
    /// The employee this dependent is associated with.
    /// </summary>
    public DefaultIdType EmployeeId { get; private set; }
    public Employee Employee { get; private set; } = default!;

    /// <summary>
    /// First name of the dependent.
    /// </summary>
    public string FirstName { get; private set; } = default!;

    /// <summary>
    /// Last name of the dependent.
    /// </summary>
    public string LastName { get; private set; } = default!;

    /// <summary>
    /// Full name of the dependent (read-only, computed).
    /// </summary>
    public string FullName => $"{FirstName} {LastName}".Trim();

    /// <summary>
    /// Type of dependent (Spouse, Child, Parent, Sibling, Other).
    /// </summary>
    public string DependentType { get; private set; } = default!;

    /// <summary>
    /// Date of birth of the dependent.
    /// </summary>
    public DateTime? DateOfBirth { get; private set; }

    /// <summary>
    /// Age of the dependent (read-only, computed).
    /// </summary>
    public int? Age => DateOfBirth.HasValue 
        ? DateTime.Today.Year - DateOfBirth.Value.Year - (DateOfBirth.Value.Date > DateTime.Today.AddYears(-(DateTime.Today.Year - DateOfBirth.Value.Year)) ? 1 : 0)
        : null;

    /// <summary>
    /// Relationship description to employee (e.g., "Biological child", "Spouse").
    /// </summary>
    public string? Relationship { get; private set; }

    /// <summary>
    /// Social Security Number (encrypted at rest).
    /// </summary>
    public string? Ssn { get; private set; }

    /// <summary>
    /// Email address of the dependent.
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Phone number of the dependent.
    /// </summary>
    public string? PhoneNumber { get; private set; }

    /// <summary>
    /// Whether this dependent is a beneficiary for benefits/insurance.
    /// </summary>
    public bool IsBeneficiary { get; private set; }

    /// <summary>
    /// Whether this dependent qualifies for tax exemption.
    /// </summary>
    public bool IsClaimableDependent { get; private set; }

    /// <summary>
    /// Date when dependent eligibility ends (e.g., aging out).
    /// </summary>
    public DateTime? EligibilityEndDate { get; private set; }

    /// <summary>
    /// Whether this dependent is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Creates a new employee dependent.
    /// </summary>
    public static EmployeeDependent Create(
        DefaultIdType employeeId,
        string firstName,
        string lastName,
        string dependentType,
        DateTime? dateOfBirth,
        string? relationship = null,
        string? ssn = null,
        string? email = null,
        string? phoneNumber = null)
    {
        var dependent = new EmployeeDependent(
            DefaultIdType.NewGuid(),
            employeeId,
            firstName,
            lastName,
            dependentType,
            dateOfBirth,
            relationship,
            ssn,
            email,
            phoneNumber);

        return dependent;
    }

    /// <summary>
    /// Updates dependent information.
    /// </summary>
    public EmployeeDependent Update(
        string? firstName = null,
        string? lastName = null,
        string? relationship = null,
        DateTime? dateOfBirth = null,
        string? email = null,
        string? phoneNumber = null)
    {
        if (!string.IsNullOrWhiteSpace(firstName))
            FirstName = firstName;

        if (!string.IsNullOrWhiteSpace(lastName))
            LastName = lastName;

        if (relationship != null)
            Relationship = relationship;

        if (dateOfBirth.HasValue)
            DateOfBirth = dateOfBirth.Value;

        if (email != null)
            Email = email;

        if (phoneNumber != null)
            PhoneNumber = phoneNumber;

        QueueDomainEvent(new EmployeeDependentUpdated { Dependent = this });
        return this;
    }

    /// <summary>
    /// Sets whether dependent is a beneficiary.
    /// </summary>
    public EmployeeDependent SetAsBeneficiary(bool isBeneficiary)
    {
        IsBeneficiary = isBeneficiary;
        QueueDomainEvent(new EmployeeDependentUpdated { Dependent = this });
        return this;
    }

    /// <summary>
    /// Sets whether dependent is claimable for tax purposes.
    /// </summary>
    public EmployeeDependent SetAsClaimableDependent(bool isClaimable)
    {
        IsClaimableDependent = isClaimable;
        QueueDomainEvent(new EmployeeDependentUpdated { Dependent = this });
        return this;
    }

    /// <summary>
    /// Sets the eligibility end date.
    /// </summary>
    public EmployeeDependent SetEligibilityEndDate(DateTime? endDate)
    {
        EligibilityEndDate = endDate;
        QueueDomainEvent(new EmployeeDependentUpdated { Dependent = this });
        return this;
    }

    /// <summary>
    /// Deactivates this dependent.
    /// </summary>
    public EmployeeDependent Deactivate()
    {
        IsActive = false;
        QueueDomainEvent(new EmployeeDependentDeactivated { DependentId = Id });
        return this;
    }

    /// <summary>
    /// Activates this dependent.
    /// </summary>
    public EmployeeDependent Activate()
    {
        IsActive = true;
        QueueDomainEvent(new EmployeeDependentActivated { DependentId = Id });
        return this;
    }
}

/// <summary>
/// Dependent type constants.
/// </summary>
public static class DependentType
{
    public const string Spouse = "Spouse";
    public const string Child = "Child";
    public const string Parent = "Parent";
    public const string Sibling = "Sibling";
    public const string Other = "Other";
}
