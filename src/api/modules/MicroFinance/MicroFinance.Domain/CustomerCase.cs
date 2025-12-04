using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a customer service case or complaint.
/// </summary>
public sealed class CustomerCase : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int CaseNumberMaxLength = 32;
    public const int SubjectMaxLength = 256;
    public const int CategoryMaxLength = 64;
    public const int PriorityMaxLength = 32;
    public const int StatusMaxLength = 32;
    public const int DescriptionMaxLength = 2048;
    public const int ResolutionMaxLength = 2048;
    public const int ChannelMaxLength = 32;
    
    // Case Status
    public const string StatusOpen = "Open";
    public const string StatusInProgress = "InProgress";
    public const string StatusPending = "Pending";
    public const string StatusResolved = "Resolved";
    public const string StatusClosed = "Closed";
    public const string StatusEscalated = "Escalated";
    
    // Priority Levels
    public const string PriorityLow = "Low";
    public const string PriorityMedium = "Medium";
    public const string PriorityHigh = "High";
    public const string PriorityCritical = "Critical";
    
    // Case Categories
    public const string CategoryComplaint = "Complaint";
    public const string CategoryInquiry = "Inquiry";
    public const string CategoryRequest = "Request";
    public const string CategoryDispute = "Dispute";
    public const string CategoryFeedback = "Feedback";
    
    // Channels
    public const string ChannelPhone = "Phone";
    public const string ChannelEmail = "Email";
    public const string ChannelWalkIn = "WalkIn";
    public const string ChannelUssd = "USSD";
    public const string ChannelApp = "MobileApp";
    public const string ChannelWeb = "Web";

    public string CaseNumber { get; private set; } = default!;
    public Guid MemberId { get; private set; }
    public string Subject { get; private set; } = default!;
    public string Category { get; private set; } = default!;
    public string Priority { get; private set; } = PriorityMedium;
    public string Status { get; private set; } = StatusOpen;
    public string Description { get; private set; } = default!;
    public string Channel { get; private set; } = default!;
    public Guid? AssignedToId { get; private set; }
    public Guid? RelatedLoanId { get; private set; }
    public Guid? RelatedSavingsAccountId { get; private set; }
    public DateTimeOffset OpenedAt { get; private set; }
    public DateTimeOffset? FirstResponseAt { get; private set; }
    public DateTimeOffset? ResolvedAt { get; private set; }
    public DateTimeOffset? ClosedAt { get; private set; }
    public string? Resolution { get; private set; }
    public int EscalationLevel { get; private set; }
    public Guid? EscalatedToId { get; private set; }
    public DateTimeOffset? EscalatedAt { get; private set; }
    public int SlaHours { get; private set; }
    public bool SlaBreached { get; private set; }
    public int? CustomerSatisfactionScore { get; private set; }
    public string? CustomerFeedback { get; private set; }

    private CustomerCase() { }

    public static CustomerCase Create(
        string caseNumber,
        Guid memberId,
        string subject,
        string category,
        string description,
        string channel,
        string priority = PriorityMedium,
        int slaHours = 24)
    {
        var customerCase = new CustomerCase
        {
            CaseNumber = caseNumber,
            MemberId = memberId,
            Subject = subject,
            Category = category,
            Description = description,
            Channel = channel,
            Priority = priority,
            Status = StatusOpen,
            OpenedAt = DateTimeOffset.UtcNow,
            SlaHours = slaHours
        };

        customerCase.QueueDomainEvent(new CustomerCaseCreated(customerCase));
        return customerCase;
    }

    public CustomerCase Assign(Guid staffId)
    {
        AssignedToId = staffId;
        Status = StatusInProgress;
        if (!FirstResponseAt.HasValue)
        {
            FirstResponseAt = DateTimeOffset.UtcNow;
        }
        QueueDomainEvent(new CustomerCaseAssigned(Id, staffId));
        return this;
    }

    public CustomerCase Escalate(Guid escalatedToId, string reason)
    {
        EscalationLevel++;
        EscalatedToId = escalatedToId;
        EscalatedAt = DateTimeOffset.UtcNow;
        Status = StatusEscalated;
        QueueDomainEvent(new CustomerCaseEscalated(Id, EscalationLevel, reason));
        return this;
    }

    public CustomerCase Resolve(string resolution)
    {
        Status = StatusResolved;
        Resolution = resolution;
        ResolvedAt = DateTimeOffset.UtcNow;
        QueueDomainEvent(new CustomerCaseResolved(Id, resolution));
        return this;
    }

    public CustomerCase Close(int? satisfactionScore = null, string? feedback = null)
    {
        Status = StatusClosed;
        ClosedAt = DateTimeOffset.UtcNow;
        CustomerSatisfactionScore = satisfactionScore;
        CustomerFeedback = feedback;
        QueueDomainEvent(new CustomerCaseClosed(Id, satisfactionScore));
        return this;
    }

    public CustomerCase MarkSlaBreached()
    {
        SlaBreached = true;
        QueueDomainEvent(new CustomerCaseSlaBreached(Id, CaseNumber));
        return this;
    }

    public CustomerCase LinkToLoan(Guid loanId)
    {
        RelatedLoanId = loanId;
        return this;
    }

    public CustomerCase LinkToSavings(Guid savingsAccountId)
    {
        RelatedSavingsAccountId = savingsAccountId;
        return this;
    }

    public CustomerCase Update(
        string? subject = null,
        string? description = null,
        string? priority = null)
    {
        if (subject is not null) Subject = subject;
        if (description is not null) Description = description;
        if (priority is not null) Priority = priority;

        QueueDomainEvent(new CustomerCaseUpdated(this));
        return this;
    }
}
