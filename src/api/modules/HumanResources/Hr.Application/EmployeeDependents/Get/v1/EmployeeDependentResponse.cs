namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Get.v1;

/// <summary>
/// Response object for EmployeeDependent entity details.
/// </summary>
public sealed record EmployeeDependentResponse
{
    /// <summary>
    /// Gets the unique identifier of the dependent.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Gets the employee identifier.
    /// </summary>
    public DefaultIdType EmployeeId { get; init; }

    /// <summary>
    /// Gets the first name of the dependent.
    /// </summary>
    public string FirstName { get; init; } = default!;

    /// <summary>
    /// Gets the last name of the dependent.
    /// </summary>
    public string LastName { get; init; } = default!;

    /// <summary>
    /// Gets the full name of the dependent.
    /// </summary>
    public string FullName { get; init; } = default!;

    /// <summary>
    /// Gets the type of dependent (Spouse, Child, Parent, Sibling, Other).
    /// </summary>
    public string DependentType { get; init; } = default!;

    /// <summary>
    /// Gets the date of birth.
    /// </summary>
    public DateTime? DateOfBirth { get; init; }

    /// <summary>
    /// Gets the age of the dependent.
    /// </summary>
    public int? Age { get; init; }

    /// <summary>
    /// Gets the relationship description.
    /// </summary>
    public string? Relationship { get; init; }

    /// <summary>
    /// Gets the email address.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Gets the phone number.
    /// </summary>
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// Gets a value indicating whether dependent is a beneficiary.
    /// </summary>
    public bool IsBeneficiary { get; init; }

    /// <summary>
    /// Gets a value indicating whether dependent qualifies for tax exemption.
    /// </summary>
    public bool IsClaimableDependent { get; init; }

    /// <summary>
    /// Gets the eligibility end date.
    /// </summary>
    public DateTime? EligibilityEndDate { get; init; }

    /// <summary>
    /// Gets a value indicating whether the dependent is active.
    /// </summary>
    public bool IsActive { get; init; }
}

