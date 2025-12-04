using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a journal entry for double-entry accounting.
/// </summary>
public sealed class JournalEntry : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int EntryNumberMaxLength = 32;
    public const int TypeMaxLength = 32;
    public const int StatusMaxLength = 32;
    public const int DescriptionMaxLength = 512;
    public const int ReferenceMaxLength = 128;
    
    // Entry Types
    public const string TypeManual = "Manual";
    public const string TypeAutomatic = "Automatic";
    public const string TypeAdjusting = "Adjusting";
    public const string TypeClosing = "Closing";
    public const string TypeReversal = "Reversal";
    
    // Entry Status
    public const string StatusDraft = "Draft";
    public const string StatusPending = "Pending";
    public const string StatusApproved = "Approved";
    public const string StatusPosted = "Posted";
    public const string StatusReversed = "Reversed";

    public string EntryNumber { get; private set; } = default!;
    public string EntryType { get; private set; } = TypeManual;
    public string Status { get; private set; } = StatusDraft;
    public DateOnly EntryDate { get; private set; }
    public DateOnly? PostingDate { get; private set; }
    public string Description { get; private set; } = default!;
    public string? Reference { get; private set; }
    public decimal TotalDebit { get; private set; }
    public decimal TotalCredit { get; private set; }
    public bool IsBalanced { get; private set; }
    public Guid? SourceEntityType { get; private set; }
    public Guid? SourceEntityId { get; private set; }
    public Guid? BranchId { get; private set; }
    public Guid? CreatedById { get; private set; }
    public Guid? ApprovedById { get; private set; }
    public DateTimeOffset? ApprovedAt { get; private set; }
    public Guid? PostedById { get; private set; }
    public DateTimeOffset? PostedAt { get; private set; }
    public Guid? ReversalOfEntryId { get; private set; }
    public Guid? ReversedByEntryId { get; private set; }
    public string? Notes { get; private set; }

    private JournalEntry() { }

    public static JournalEntry Create(
        string entryNumber,
        DateOnly entryDate,
        string description,
        string entryType = TypeManual,
        string? reference = null,
        Guid? branchId = null)
    {
        var entry = new JournalEntry
        {
            EntryNumber = entryNumber,
            EntryDate = entryDate,
            Description = description,
            EntryType = entryType,
            Reference = reference,
            BranchId = branchId,
            Status = StatusDraft,
            TotalDebit = 0,
            TotalCredit = 0,
            IsBalanced = true
        };

        entry.QueueDomainEvent(new JournalEntryCreated(entry));
        return entry;
    }

    public JournalEntry AddDebit(decimal amount)
    {
        TotalDebit += amount;
        UpdateBalance();
        return this;
    }

    public JournalEntry AddCredit(decimal amount)
    {
        TotalCredit += amount;
        UpdateBalance();
        return this;
    }

    private void UpdateBalance()
    {
        IsBalanced = TotalDebit == TotalCredit;
    }

    public JournalEntry Submit()
    {
        if (!IsBalanced)
            throw new InvalidOperationException("Journal entry must be balanced before submission.");

        Status = StatusPending;
        QueueDomainEvent(new JournalEntrySubmitted(Id, EntryNumber));
        return this;
    }

    public JournalEntry Approve(Guid approvedById)
    {
        if (!IsBalanced)
            throw new InvalidOperationException("Journal entry must be balanced.");

        Status = StatusApproved;
        ApprovedById = approvedById;
        ApprovedAt = DateTimeOffset.UtcNow;
        QueueDomainEvent(new JournalEntryApproved(Id, EntryNumber));
        return this;
    }

    public JournalEntry Post(Guid postedById, DateOnly postingDate)
    {
        if (Status != StatusApproved)
            throw new InvalidOperationException("Journal entry must be approved before posting.");

        Status = StatusPosted;
        PostedById = postedById;
        PostedAt = DateTimeOffset.UtcNow;
        PostingDate = postingDate;
        QueueDomainEvent(new JournalEntryPosted(Id, EntryNumber, TotalDebit));
        return this;
    }

    public JournalEntry Reverse(Guid reversalEntryId)
    {
        Status = StatusReversed;
        ReversedByEntryId = reversalEntryId;
        QueueDomainEvent(new JournalEntryReversed(Id, reversalEntryId));
        return this;
    }

    public JournalEntry SetSource(Guid sourceEntityType, Guid sourceEntityId)
    {
        SourceEntityType = sourceEntityType;
        SourceEntityId = sourceEntityId;
        return this;
    }

    public JournalEntry Update(
        string? description = null,
        string? reference = null,
        string? notes = null)
    {
        if (Status == StatusPosted)
            throw new InvalidOperationException("Cannot update posted journal entry.");

        if (description is not null) Description = description;
        if (reference is not null) Reference = reference;
        if (notes is not null) Notes = notes;

        QueueDomainEvent(new JournalEntryUpdated(this));
        return this;
    }
}
