using Accounting.Domain.Events.Member;

namespace Accounting.Domain;

/// <summary>
/// Represents a utility member/customer account with service location, contact info, rate schedule, and balance.
/// </summary>
/// <remarks>
/// Tracks account status lifecycle, assigned meter, balance, and membership date. Defaults: <see cref="IsActive"/> true on create,
/// <see cref="CurrentBalance"/> 0, <see cref="AccountStatus"/> "Active" unless provided.
/// </remarks>
public class Member : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique identifier assigned to the member.
    /// </summary>
    public string MemberNumber { get; private set; } // MemberID equivalent

    /// <summary>
    /// Human-readable name for the member account.
    /// </summary>
    public string MemberName { get; private set; }

    /// <summary>
    /// Service location where power is supplied.
    /// </summary>
    public string ServiceAddress { get; private set; } // Physical location where power is supplied

    /// <summary>
    /// Mailing address for correspondence.
    /// </summary>
    public string? MailingAddress { get; private set; } // Member's mailing address

    /// <summary>
    /// General contact information (phone, email), if not using <see cref="Email"/> and <see cref="PhoneNumber"/> fields.
    /// </summary>
    public string? ContactInfo { get; private set; } // Phone, email, etc.

    /// <summary>
    /// Account status string (Active, Inactive, Past Due, Suspended, Closed).
    /// </summary>
    public string AccountStatus { get; private set; } // "Active", "Inactive", "Past Due"

    /// <summary>
    /// Current meter assigned to this member, if any.
    /// </summary>
    public DefaultIdType? MeterId { get; private set; } // Links to specific physical meter

    /// <summary>
    /// Rate schedule identifier (new model). Optional.
    /// </summary>
    public DefaultIdType? RateScheduleId { get; private set; }

    /// <summary>
    /// Date when membership was established.
    /// </summary>
    public DateTime MembershipDate { get; private set; }

    /// <summary>
    /// Current outstanding balance for the account.
    /// </summary>
    public decimal CurrentBalance { get; private set; }

    /// <summary>
    /// Whether the member account is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Primary email address for the member.
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Primary phone number.
    /// </summary>
    public string? PhoneNumber { get; private set; }

    /// <summary>
    /// Contact person or secondary emergency contact.
    /// </summary>
    public string? EmergencyContact { get; private set; }

    /// <summary>
    /// Service class such as Residential, Commercial, Industrial.
    /// </summary>
    public string? ServiceClass { get; private set; } // Residential, Commercial, Industrial

    /// <summary>
    /// Legacy human-readable rate schedule text; prefer <see cref="RateScheduleId"/> for structured link.
    /// </summary>
    public string? RateSchedule { get; private set; } // (legacy) Rate schedule applied (human-readable)

    private Member()
    {
        // EF Core requires a parameterless constructor for entity instantiation
    }

    private Member(string memberNumber, string memberName, string serviceAddress,
        DateTime membershipDate, string? mailingAddress = null, string? contactInfo = null,
        string accountStatus = "Active", DefaultIdType? meterId = null, string? email = null,
        string? phoneNumber = null, string? emergencyContact = null, string? serviceClass = null,
        DefaultIdType? rateScheduleId = null, string? rateSchedule = null, string? description = null, string? notes = null)
    {
        MemberNumber = memberNumber.Trim();
        MemberName = memberName.Trim();
        Name = memberName.Trim(); // Keep for compatibility
        ServiceAddress = serviceAddress.Trim();
        MembershipDate = membershipDate;
        MailingAddress = mailingAddress?.Trim();
        ContactInfo = contactInfo?.Trim();
        AccountStatus = accountStatus.Trim();
        MeterId = meterId;
        CurrentBalance = 0;
        IsActive = true;
        Email = email?.Trim();
        PhoneNumber = phoneNumber?.Trim();
        EmergencyContact = emergencyContact?.Trim();
        ServiceClass = serviceClass?.Trim();
        RateScheduleId = rateScheduleId;
        RateSchedule = rateSchedule?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new MemberCreated(Id, MemberNumber, MemberName, ServiceAddress, MembershipDate, Description, Notes));
    }

    /// <summary>
    /// Factory to create a new member with validation for key fields and sensible defaults.
    /// </summary>
    public static Member Create(string memberNumber, string memberName, string serviceAddress,
        DateTime membershipDate, string? mailingAddress = null, string? contactInfo = null,
        string accountStatus = "Active", DefaultIdType? meterId = null, string? email = null,
        string? phoneNumber = null, string? emergencyContact = null, string? serviceClass = null,
        DefaultIdType? rateScheduleId = null, string? rateSchedule = null, string? description = null, string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(memberNumber))
            throw new MemberNotFoundException(DefaultIdType.Empty);
            
        if (string.IsNullOrWhiteSpace(memberName))
            throw new ArgumentException("Member name cannot be empty");
            
        if (string.IsNullOrWhiteSpace(serviceAddress))
            throw new ArgumentException("Service address cannot be empty");

        if (!IsValidAccountStatus(accountStatus))
            throw new ArgumentException($"Invalid account status: {accountStatus}");

        return new Member(memberNumber, memberName, serviceAddress, membershipDate,
            mailingAddress, contactInfo, accountStatus, meterId, email, phoneNumber, emergencyContact,
            serviceClass, rateScheduleId, rateSchedule, description, notes);
    }

    /// <summary>
    /// Update member metadata; trims inputs and enforces valid statuses.
    /// </summary>
    public Member Update(string? memberName = null, string? serviceAddress = null, string? mailingAddress = null,
        string? contactInfo = null, string? accountStatus = null, DefaultIdType? meterId = null,
        string? email = null, string? phoneNumber = null, string? emergencyContact = null,
        string? serviceClass = null, string? rateSchedule = null, string? description = null, string? notes = null)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(memberName) && MemberName != memberName.Trim())
        {
            MemberName = memberName.Trim();
            Name = MemberName; // Keep for compatibility
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(serviceAddress) && ServiceAddress != serviceAddress.Trim())
        {
            ServiceAddress = serviceAddress.Trim();
            isUpdated = true;
        }

        if (mailingAddress != MailingAddress)
        {
            MailingAddress = mailingAddress?.Trim();
            isUpdated = true;
        }

        if (contactInfo != ContactInfo)
        {
            ContactInfo = contactInfo?.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(accountStatus) && AccountStatus != accountStatus.Trim())
        {
            if (!IsValidAccountStatus(accountStatus))
                throw new ArgumentException($"Invalid account status: {accountStatus}");
            AccountStatus = accountStatus.Trim();
            isUpdated = true;
        }

        if (meterId != MeterId)
        {
            MeterId = meterId;
            isUpdated = true;
        }

        if (email != Email)
        {
            Email = email?.Trim();
            isUpdated = true;
        }

        if (phoneNumber != PhoneNumber)
        {
            PhoneNumber = phoneNumber?.Trim();
            isUpdated = true;
        }

        if (emergencyContact != EmergencyContact)
        {
            EmergencyContact = emergencyContact?.Trim();
            isUpdated = true;
        }

        if (serviceClass != ServiceClass)
        {
            ServiceClass = serviceClass?.Trim();
            isUpdated = true;
        }

        // Support updating either the legacy RateSchedule text or the new RateScheduleId via API-level mapping
        if (rateSchedule != RateSchedule)
        {
            RateSchedule = rateSchedule?.Trim();
            isUpdated = true;
        }

        if (description != Description)
        {
            Description = description?.Trim();
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new MemberUpdated(Id, MemberNumber, MemberName, ServiceAddress, Description, Notes));
        }

        return this;
    }

    /// <summary>
    /// Replace current balance and emit a balance-updated event.
    /// </summary>
    public Member UpdateBalance(decimal newBalance)
    {
        if (CurrentBalance != newBalance)
        {
            CurrentBalance = newBalance;
            QueueDomainEvent(new MemberBalanceUpdated(Id, MemberNumber, CurrentBalance));
        }
        return this;
    }

    /// <summary>
    /// Activate the member account and set status to Active.
    /// </summary>
    public Member Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            AccountStatus = "Active";
            QueueDomainEvent(new MemberStatusChanged(Id, MemberNumber, IsActive, AccountStatus));
        }
        return this;
    }

    /// <summary>
    /// Deactivate the member account and set status to Inactive.
    /// </summary>
    public Member Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            AccountStatus = "Inactive";
            QueueDomainEvent(new MemberStatusChanged(Id, MemberNumber, IsActive, AccountStatus));
        }
        return this;
    }

    /// <summary>
    /// Mark the account as Past Due (does not change IsActive flag).
    /// </summary>
    public Member MarkPastDue()
    {
        if (AccountStatus != "Past Due")
        {
            AccountStatus = "Past Due";
            QueueDomainEvent(new MemberStatusChanged(Id, MemberNumber, IsActive, AccountStatus));
        }
        return this;
    }

    private static bool IsValidAccountStatus(string accountStatus)
    {
        var validStatuses = new[] { "Active", "Inactive", "Past Due", "Suspended", "Closed" };
        return validStatuses.Contains(accountStatus.Trim(), StringComparer.OrdinalIgnoreCase);
    }
}
