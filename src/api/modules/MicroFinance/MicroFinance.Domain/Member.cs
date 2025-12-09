using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a member (client) in the microfinance system.
/// </summary>
/// <remarks>
/// Use cases:
/// - Register new members with KYC (Know Your Customer) information.
/// - Maintain member profiles with contact and demographic data.
/// - Track member's financial relationship (loans, savings, shares).
/// - Assess creditworthiness based on income and history.
/// - Manage member lifecycle (activation, deactivation).
/// - Support group lending through solidarity group membership.
/// - Enable loan guarantorship between members.
/// - Generate member statements and account summaries.
/// 
/// Default values and constraints:
/// - MemberNumber: required unique identifier, max 64 characters (example: "MBR-2024-001234")
/// - FirstName: required, 2-128 characters
/// - LastName: required, 2-128 characters
/// - Email: optional, max 256 characters, must be valid email format
/// - PhoneNumber: optional, max 32 characters
/// - NationalId: recommended for KYC compliance, max 64 characters
/// - MonthlyIncome: optional, must be non-negative if provided
/// - DateOfBirth: optional, must be in the past if provided
/// - IsActive: true by default
/// - JoinDate: current UTC date if not specified
/// 
/// Business rules:
/// - MemberNumber must be unique within the system.
/// - Member must pass KYC verification before activation.
/// - Cannot delete members with active loans or positive account balances.
/// - Members can have multiple savings accounts but limited active loans per policy.
/// - Income verification required for loan amounts above threshold.
/// - National ID or equivalent document required for regulatory compliance.
/// - Group membership required for group lending products.
/// </remarks>
/// <seealso cref="Loan"/>
/// <seealso cref="SavingsAccount"/>
/// <seealso cref="ShareAccount"/>
/// <seealso cref="FixedDeposit"/>
/// <seealso cref="GroupMembership"/>
/// <seealso cref="LoanGuarantor"/>
/// <seealso cref="KycDocument"/>
public class Member : AuditableEntity, IAggregateRoot
{
    // Domain Constants - Binary Limits (Powers of 2)
    /// <summary>Maximum length for member number field. (2^6 = 64)</summary>
    public const int MemberNumberMaxLength = 64;

    /// <summary>Maximum length for first name field. (2^7 = 128)</summary>
    public const int FirstNameMaxLength = 128;

    /// <summary>Maximum length for last name field. (2^7 = 128)</summary>
    public const int LastNameMaxLength = 128;

    /// <summary>Maximum length for middle name field. (2^7 = 128)</summary>
    public const int MiddleNameMaxLength = 128;

    /// <summary>Maximum length for email field. (2^8 = 256)</summary>
    public const int EmailMaxLength = 256;

    /// <summary>Maximum length for phone number field. (2^5 = 32)</summary>
    public const int PhoneNumberMaxLength = 32;

    /// <summary>Maximum length for address field. (2^9 = 512)</summary>
    public const int AddressMaxLength = 512;

    /// <summary>Maximum length for city field. (2^7 = 128)</summary>
    public const int CityMaxLength = 128;

    /// <summary>Maximum length for state/province field. (2^7 = 128)</summary>
    public const int StateMaxLength = 128;

    /// <summary>Maximum length for postal code field. (2^5 = 32)</summary>
    public const int PostalCodeMaxLength = 32;

    /// <summary>Maximum length for country field. (2^7 = 128)</summary>
    public const int CountryMaxLength = 128;

    /// <summary>Maximum length for national ID field. (2^6 = 64)</summary>
    public const int NationalIdMaxLength = 64;

    /// <summary>Maximum length for occupation field. (2^8 = 256)</summary>
    public const int OccupationMaxLength = 256;

    /// <summary>Maximum length for gender field. (2^5 = 32)</summary>
    public const int GenderMaxLength = 32;

    /// <summary>Maximum length for notes field. (2^12 = 4096)</summary>
    public const int NotesMaxLength = 4096;

    /// <summary>Minimum length for first name field.</summary>
    public const int FirstNameMinLength = 2;

    /// <summary>Minimum length for last name field.</summary>
    public const int LastNameMinLength = 2;

    /// <summary>Gets the unique member number.</summary>
    public string MemberNumber { get; private set; } = default!;

    /// <summary>Gets the member's first name.</summary>
    public string FirstName { get; private set; } = default!;

    /// <summary>Gets the member's last name.</summary>
    public string LastName { get; private set; } = default!;

    /// <summary>Gets the member's middle name.</summary>
    public string? MiddleName { get; private set; }

    /// <summary>Gets the member's full name.</summary>
    public string FullName => string.IsNullOrWhiteSpace(MiddleName)
        ? $"{FirstName} {LastName}"
        : $"{FirstName} {MiddleName} {LastName}";

    /// <summary>Gets the member's email address.</summary>
    public string? Email { get; private set; }

    /// <summary>Gets the member's phone number.</summary>
    public string? PhoneNumber { get; private set; }

    /// <summary>Gets the member's date of birth.</summary>
    public DateOnly? DateOfBirth { get; private set; }

    /// <summary>Gets the member's gender.</summary>
    public string? Gender { get; private set; }

    /// <summary>Gets the member's address.</summary>
    public string? Address { get; private set; }

    /// <summary>Gets the member's city.</summary>
    public string? City { get; private set; }

    /// <summary>Gets the member's state or province.</summary>
    public string? State { get; private set; }

    /// <summary>Gets the member's postal code.</summary>
    public string? PostalCode { get; private set; }

    /// <summary>Gets the member's country.</summary>
    public string? Country { get; private set; }

    /// <summary>Gets the member's national ID number.</summary>
    public string? NationalId { get; private set; }

    /// <summary>Gets the member's occupation.</summary>
    public string? Occupation { get; private set; }

    /// <summary>Gets the monthly income of the member.</summary>
    public decimal? MonthlyIncome { get; private set; }

    /// <summary>Gets the date the member joined.</summary>
    public DateOnly JoinDate { get; private set; }

    /// <summary>Gets a value indicating whether the member is active.</summary>
    public bool IsActive { get; private set; }

    /// <summary>Gets the collection of loans for this member.</summary>
    public virtual ICollection<Loan> Loans { get; private set; } = new List<Loan>();

    /// <summary>Gets the collection of savings accounts for this member.</summary>
    public virtual ICollection<SavingsAccount> SavingsAccounts { get; private set; } = new List<SavingsAccount>();

    private Member() { }

    private Member(
        DefaultIdType id,
        string memberNumber,
        string firstName,
        string lastName,
        string? middleName,
        string? email,
        string? phoneNumber,
        DateOnly? dateOfBirth,
        string? gender,
        string? address,
        string? city,
        string? state,
        string? postalCode,
        string? country,
        string? nationalId,
        string? occupation,
        decimal? monthlyIncome,
        DateOnly joinDate,
        string? notes)
    {
        Id = id;
        MemberNumber = memberNumber.Trim();
        ValidateAndSetFirstName(firstName);
        ValidateAndSetLastName(lastName);
        MiddleName = middleName?.Trim();
        Email = email?.Trim();
        PhoneNumber = phoneNumber?.Trim();
        DateOfBirth = dateOfBirth;
        Gender = gender?.Trim();
        Address = address?.Trim();
        City = city?.Trim();
        State = state?.Trim();
        PostalCode = postalCode?.Trim();
        Country = country?.Trim();
        NationalId = nationalId?.Trim();
        Occupation = occupation?.Trim();
        MonthlyIncome = monthlyIncome;
        JoinDate = joinDate;
        IsActive = true;
        Notes = notes?.Trim();

        QueueDomainEvent(new MemberCreated { Member = this });
    }

    /// <summary>
    /// Creates a new Member instance using the factory method pattern.
    /// </summary>
    public static Member Create(
        string memberNumber,
        string firstName,
        string lastName,
        string? middleName = null,
        string? email = null,
        string? phoneNumber = null,
        DateOnly? dateOfBirth = null,
        string? gender = null,
        string? address = null,
        string? city = null,
        string? state = null,
        string? postalCode = null,
        string? country = null,
        string? nationalId = null,
        string? occupation = null,
        decimal? monthlyIncome = null,
        DateOnly? joinDate = null,
        string? notes = null)
    {
        return new Member(
            DefaultIdType.NewGuid(),
            memberNumber,
            firstName,
            lastName,
            middleName,
            email,
            phoneNumber,
            dateOfBirth,
            gender,
            address,
            city,
            state,
            postalCode,
            country,
            nationalId,
            occupation,
            monthlyIncome,
            joinDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            notes);
    }

    /// <summary>
    /// Updates the member's information.
    /// </summary>
    public Member Update(
        string? firstName,
        string? lastName,
        string? middleName,
        string? email,
        string? phoneNumber,
        DateOnly? dateOfBirth,
        string? gender,
        string? address,
        string? city,
        string? state,
        string? postalCode,
        string? country,
        string? nationalId,
        string? occupation,
        decimal? monthlyIncome,
        string? notes)
    {
        bool hasChanges = false;

        if (!string.IsNullOrWhiteSpace(firstName) && !string.Equals(FirstName, firstName.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            ValidateAndSetFirstName(firstName);
            hasChanges = true;
        }

        if (!string.IsNullOrWhiteSpace(lastName) && !string.Equals(LastName, lastName.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            ValidateAndSetLastName(lastName);
            hasChanges = true;
        }

        if (middleName != MiddleName) { MiddleName = middleName?.Trim(); hasChanges = true; }
        if (email != Email) { Email = email?.Trim(); hasChanges = true; }
        if (phoneNumber != PhoneNumber) { PhoneNumber = phoneNumber?.Trim(); hasChanges = true; }
        if (dateOfBirth != DateOfBirth) { DateOfBirth = dateOfBirth; hasChanges = true; }
        if (gender != Gender) { Gender = gender?.Trim(); hasChanges = true; }
        if (address != Address) { Address = address?.Trim(); hasChanges = true; }
        if (city != City) { City = city?.Trim(); hasChanges = true; }
        if (state != State) { State = state?.Trim(); hasChanges = true; }
        if (postalCode != PostalCode) { PostalCode = postalCode?.Trim(); hasChanges = true; }
        if (country != Country) { Country = country?.Trim(); hasChanges = true; }
        if (nationalId != NationalId) { NationalId = nationalId?.Trim(); hasChanges = true; }
        if (occupation != Occupation) { Occupation = occupation?.Trim(); hasChanges = true; }
        if (monthlyIncome != MonthlyIncome) { MonthlyIncome = monthlyIncome; hasChanges = true; }
        if (notes != Notes) { Notes = notes?.Trim(); hasChanges = true; }

        if (hasChanges)
        {
            QueueDomainEvent(new MemberUpdated { Member = this });
        }

        return this;
    }

    /// <summary>Activates the member.</summary>
    public Member Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new MemberActivated { MemberId = Id });
        }
        return this;
    }

    /// <summary>Deactivates the member.</summary>
    public Member Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            QueueDomainEvent(new MemberDeactivated { MemberId = Id });
        }
        return this;
    }

    private void ValidateAndSetFirstName(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));

        string trimmed = firstName.Trim();
        if (trimmed.Length < FirstNameMinLength)
            throw new ArgumentException($"First name must be at least {FirstNameMinLength} characters.", nameof(firstName));
        if (trimmed.Length > FirstNameMaxLength)
            throw new ArgumentException($"First name cannot exceed {FirstNameMaxLength} characters.", nameof(firstName));

        FirstName = trimmed;
    }

    private void ValidateAndSetLastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

        string trimmed = lastName.Trim();
        if (trimmed.Length < LastNameMinLength)
            throw new ArgumentException($"Last name must be at least {LastNameMinLength} characters.", nameof(lastName));
        if (trimmed.Length > LastNameMaxLength)
            throw new ArgumentException($"Last name cannot exceed {LastNameMaxLength} characters.", nameof(lastName));

        LastName = trimmed;
    }
}

