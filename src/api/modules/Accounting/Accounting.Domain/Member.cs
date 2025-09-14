using Accounting.Domain.Events.Member;

namespace Accounting.Domain;

public class Member : AuditableEntity, IAggregateRoot
{
    public string MemberNumber { get; private set; } // MemberID equivalent
    public string MemberName { get; private set; }
    public string ServiceAddress { get; private set; } // Physical location where power is supplied
    public string? MailingAddress { get; private set; } // Member's mailing address
    public string? ContactInfo { get; private set; } // Phone, email, etc.
    public string AccountStatus { get; private set; } // "Active", "Inactive", "Past Due"
    public DefaultIdType? MeterId { get; private set; } // Links to specific physical meter
    // Link to RateSchedule entity (new)
    public DefaultIdType? RateScheduleId { get; private set; }
    public DateTime MembershipDate { get; private set; }
    public decimal CurrentBalance { get; private set; }
    public bool IsActive { get; private set; }
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? EmergencyContact { get; private set; }
    public string? ServiceClass { get; private set; } // Residential, Commercial, Industrial
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

    public Member UpdateBalance(decimal newBalance)
    {
        if (CurrentBalance != newBalance)
        {
            CurrentBalance = newBalance;
            QueueDomainEvent(new MemberBalanceUpdated(Id, MemberNumber, CurrentBalance));
        }
        return this;
    }

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
