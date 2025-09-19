using Accounting.Domain.Events.Member;

namespace Accounting.Domain;

/// <summary>
/// Represents a utility member/customer account with service location, contact information, billing details, and account lifecycle management.
/// </summary>
/// <remarks>
/// Use cases:
/// - Manage utility member accounts for electric cooperative or municipal utility operations.
/// - Track service locations and meter assignments for accurate billing and service delivery.
/// - Maintain member contact information for communications and emergency notifications.
/// - Support account lifecycle management (activation, suspension, disconnection, reconnection).
/// - Enable rate schedule assignments and billing calculations based on member classifications.
/// - Track account balances, payment history, and credit management.
/// - Support regulatory reporting for member demographics and service statistics.
/// - Manage member deposits, connection fees, and patronage capital allocations.
/// 
/// Default values:
/// - MemberNumber: required unique identifier (example: "M-2025-001234")
/// - MemberName: required member name (example: "John Smith" or "ABC Manufacturing Inc.")
/// - ServiceAddress: required service location (example: "123 Main St, Anytown, ST 12345")
/// - MailingAddress: optional separate mailing address (example: "PO Box 456, Anytown, ST 12345")
/// - ContactInfo: optional general contact information (example: "phone: 555-1234, email: john@example.com")
/// - AccountStatus: "Active" (new accounts start as active)
/// - MeterId: null (meter assigned separately after account creation)
/// - RateScheduleId: null (rate schedule assigned based on service type and usage)
/// - CurrentBalance: 0.00 (no initial balance)
/// - IsActive: true (accounts are active by default)
/// - MembershipDate: account creation date
/// 
/// Business rules:
/// - MemberNumber must be unique within the utility system
/// - Service address is required for utility service delivery
/// - Account status controls billing and service availability
/// - Cannot delete members with transaction history
/// - Meter assignments must be validated for service location
/// - Rate schedule changes require effective dating
/// - Balance adjustments require proper authorization
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.Member.MemberCreated"/>
/// <seealso cref="Accounting.Domain.Events.Member.MemberUpdated"/>
/// <seealso cref="Accounting.Domain.Events.Member.MemberActivated"/>
/// <seealso cref="Accounting.Domain.Events.Member.MemberDeactivated"/>
/// <seealso cref="Accounting.Domain.Events.Member.MemberMeterAssigned"/>
/// <seealso cref="Accounting.Domain.Events.Member.MemberRateScheduleChanged"/>
/// <seealso cref="Accounting.Domain.Events.Member.MemberBalanceAdjusted"/>
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
