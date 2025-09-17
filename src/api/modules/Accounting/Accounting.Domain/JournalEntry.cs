using Accounting.Domain.Events.JournalEntry;

namespace Accounting.Domain;

/// <summary>
/// Represents a journal entry grouping one or more debit/credit lines for posting to the general ledger.
/// </summary>
/// <remarks>
/// Tracks posting status, approval workflow, and ensures the entry is balanced before posting.
/// Defaults: <see cref="IsPosted"/> is false; <see cref="ApprovalStatus"/> starts as "Pending".
/// </remarks>
public class JournalEntry : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Effective date of the journal entry.
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// External reference or document number.
    /// </summary>
    public string ReferenceNumber { get; private set; }

    /// <summary>
    /// Source system or module that created the entry.
    /// </summary>
    public string Source { get; private set; }

    /// <summary>
    /// Indicates whether the entry has been posted to the general ledger.
    /// </summary>
    public bool IsPosted { get; private set; }

    /// <summary>
    /// Optional accounting period identifier to which this entry belongs.
    /// </summary>
    public DefaultIdType? PeriodId { get; private set; }

    /// <summary>
    /// Original amount for reference or control purposes; not used for balancing.
    /// </summary>
    public decimal OriginalAmount { get; private set; }
    
    private readonly List<JournalEntryLine> _lines = new();
    /// <summary>
    /// The debit/credit lines that make up this journal entry.
    /// </summary>
    public IReadOnlyCollection<JournalEntryLine> Lines => _lines.AsReadOnly();
    
    /// <summary>
    /// Approval state: Pending, Approved, or Rejected.
    /// </summary>
    public string ApprovalStatus { get; private set; } // Pending, Approved, Rejected

    /// <summary>
    /// Approver identifier/name when approved or rejected.
    /// </summary>
    public string? ApprovedBy { get; private set; }

    /// <summary>
    /// Date/time when approved or rejected.
    /// </summary>
    public DateTime? ApprovedDate { get; private set; }

    private JournalEntry()
    {
        // EF Core requires a parameterless constructor for entity instantiation
    }

    private JournalEntry(DateTime date, string referenceNumber, string description, string source,
        DefaultIdType? periodId = null, decimal originalAmount = 0)
    {
        Date = date;
        ReferenceNumber = referenceNumber.Trim()
;
        Description = description.Trim();
        Source = source.Trim();
        IsPosted = false;
        PeriodId = periodId;
        OriginalAmount = originalAmount;
        ApprovalStatus = "Pending";

        QueueDomainEvent(new JournalEntryCreated(Id, Date, ReferenceNumber, Description, Source));
    }

    /// <summary>
    /// Create a new journal entry with meta and optional control amount.
    /// </summary>
    public static JournalEntry Create(DateTime date, string referenceNumber, string description, string source,
        DefaultIdType? periodId = null, decimal originalAmount = 0)
    {
        return new JournalEntry(date, referenceNumber, description, source, periodId, originalAmount);
    }

    /// <summary>
    /// Update mutable metadata; denied if already posted.
    /// </summary>
    public JournalEntry Update(DateTime? date, string? referenceNumber, string? description, string? source,
        DefaultIdType? periodId, decimal? originalAmount)
    {
        bool isUpdated = false;

        if (IsPosted)
            throw new JournalEntryCannotBeModifiedException(Id);

        if (date.HasValue && Date != date.Value)
        {
            Date = date.Value;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(referenceNumber) && ReferenceNumber != referenceNumber)
        {
            ReferenceNumber = referenceNumber.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(description) && Description != description)
        {
            Description = description.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(source) && Source != source)
        {
            Source = source.Trim();
            isUpdated = true;
        }

        if (periodId != PeriodId)
        {
            PeriodId = periodId;
            isUpdated = true;
        }

        if (originalAmount.HasValue && OriginalAmount != originalAmount.Value)
        {
            OriginalAmount = originalAmount.Value;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new JournalEntryUpdated(this));
        }

        return this;
    }

    /// <summary>
    /// Add a debit or credit line to the entry; denied if posted.
    /// </summary>
    public JournalEntry AddLine(DefaultIdType accountId, decimal debitAmount, decimal creditAmount, string? description = null)
    {
        if (IsPosted)
            throw new JournalEntryCannotBeModifiedException(Id);

        var line = JournalEntryLine.Create(Id, accountId, debitAmount, creditAmount, description);
        _lines.Add(line);
        
        QueueDomainEvent(new JournalEntryLineAdded(Id, line.Id, accountId, debitAmount, creditAmount));
        return this;
    }

    /// <summary>
    /// Post the journal entry after verifying it is balanced (total debits equal total credits).
    /// </summary>
    public JournalEntry Post()
    {
        if (IsPosted)
            throw new JournalEntryAlreadyPostedException(Id);

        if (!IsBalanced())
            throw new JournalEntryNotBalancedException(Id);

        IsPosted = true;
        QueueDomainEvent(new JournalEntryPosted(Id, Date));
        return this;
    }

    /// <summary>
    /// Reverse a posted journal entry by queuing a reversal; does not mutate existing lines.
    /// </summary>
    public JournalEntry Reverse(DateTime reversalDate, string reversalReason)
    {
        if (!IsPosted)
            throw new JournalEntryCannotBeModifiedException(Id);

        QueueDomainEvent(new JournalEntryReversed(Id, reversalDate, reversalReason));
        return this;
    }

    /// <summary>
    /// Approve the journal entry, setting approver and timestamp.
    /// </summary>
    public void Approve(string approvedBy)
    {
        if (ApprovalStatus == "Approved")
            throw new InvalidOperationException("Journal entry already approved.");
        ApprovalStatus = "Approved";
        ApprovedBy = approvedBy;
        ApprovedDate = DateTime.UtcNow;
        QueueDomainEvent(new JournalEntryApproved(Id, ApprovedBy, ApprovedDate.Value));
    }

    /// <summary>
    /// Reject the journal entry, setting reviewer and timestamp.
    /// </summary>
    public void Reject(string rejectedBy)
    {
        if (ApprovalStatus == "Rejected")
            throw new InvalidOperationException("Journal entry already rejected.");
        ApprovalStatus = "Rejected";
        ApprovedBy = rejectedBy;
        ApprovedDate = DateTime.UtcNow;
        QueueDomainEvent(new JournalEntryRejected(Id, ApprovedBy, ApprovedDate.Value));
    }

    private bool IsBalanced()
    {
        var totalDebits = _lines.Sum(l => l.DebitAmount);
        var totalCredits = _lines.Sum(l => l.CreditAmount);
        return Math.Abs(totalDebits - totalCredits) < 0.01m;
    }
}

/// <summary>
/// A line in a journal entry representing either a debit or credit to an account.
/// </summary>
public class JournalEntryLine : BaseEntity
{
    /// <summary>
    /// Parent journal entry identifier.
    /// </summary>
    public DefaultIdType JournalEntryId { get; private set; }

    /// <summary>
    /// Account identifier to be debited or credited.
    /// </summary>
    public DefaultIdType AccountId { get; private set; }

    /// <summary>
    /// Debit amount; either this or <see cref="CreditAmount"/> must be set, but not both.
    /// </summary>
    public decimal DebitAmount { get; private set; }

    /// <summary>
    /// Credit amount; either this or <see cref="DebitAmount"/> must be set, but not both.
    /// </summary>
    public decimal CreditAmount { get; private set; }

    /// <summary>
    /// Optional description for this line.
    /// </summary>
    public string? Description { get; private set; }

    private JournalEntryLine(DefaultIdType journalEntryId, DefaultIdType accountId, 
        decimal debitAmount, decimal creditAmount, string? description = null)
    {
        JournalEntryId = journalEntryId;
        AccountId = accountId;
        DebitAmount = debitAmount;
        CreditAmount = creditAmount;
        Description = description?.Trim();
    }

    /// <summary>
    /// Create a line with validation: amounts must be non-negative and one-sided (debit XOR credit).
    /// </summary>
    public static JournalEntryLine Create(DefaultIdType journalEntryId, DefaultIdType accountId,
        decimal debitAmount, decimal creditAmount, string? description = null)
    {
        if (debitAmount < 0 || creditAmount < 0)
            throw new InvalidJournalEntryLineAmountException();

        if (debitAmount > 0 && creditAmount > 0)
            throw new InvalidJournalEntryLineAmountException();

        if (debitAmount == 0 && creditAmount == 0)
            throw new InvalidJournalEntryLineAmountException();

        return new JournalEntryLine(journalEntryId, accountId, debitAmount, creditAmount, description);
    }

    /// <summary>
    /// Update the line amounts and/or description.
    /// </summary>
    public JournalEntryLine Update(decimal? debitAmount, decimal? creditAmount, string? description)
    {
        if (debitAmount.HasValue && DebitAmount != debitAmount.Value)
        {
            DebitAmount = debitAmount.Value;
        }

        if (creditAmount.HasValue && CreditAmount != creditAmount.Value)
        {
            CreditAmount = creditAmount.Value;
        }

        if (description != Description)
        {
            Description = description?.Trim();
        }

        return this;
    }
}
