using Accounting.Domain.Events.JournalEntry;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a journal entry grouping one or more debit/credit lines for posting to the general ledger.
/// </summary>
/// <remarks>
/// Use cases:
/// - Record double-entry accounting transactions with balanced debits and credits.
/// - Support approval workflow before posting to general ledger.
/// - Track source systems and reference documents for audit trails.
/// - Enable period-based accounting and financial reporting.
/// - Maintain transaction integrity with validation rules.
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.JournalEntry.JournalEntryCreated"/>
/// <seealso cref="Accounting.Domain.Events.JournalEntry.JournalEntryUpdated"/>
/// <seealso cref="Accounting.Domain.Events.JournalEntry.JournalEntryPosted"/>
/// <seealso cref="Accounting.Domain.Events.JournalEntry.JournalEntryApproved"/>
/// <seealso cref="Accounting.Domain.Events.JournalEntry.JournalEntryRejected"/>
public class JournalEntry : AuditableEntityWithApproval, IAggregateRoot
{
    /// <summary>
    /// Effective date of the journal entry.
    /// Example: 2025-09-19 for transactions occurring on this date.
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// External reference or document number.
    /// Example: "INV-2025-001", "CHECK-12345". Used for cross-referencing source documents.
    /// </summary>
    public string ReferenceNumber { get; private set; }

    /// <summary>
    /// Source system or module that created the entry.
    /// Example: "BillingSystem", "PayrollModule", "ManualEntry". Used for audit and reconciliation.
    /// </summary>
    public string Source { get; private set; }

    /// <summary>
    /// Indicates whether the entry has been posted to the general ledger.
    /// Default: false. Once posted, the entry becomes immutable.
    /// </summary>
    public bool IsPosted { get; private set; }

    /// <summary>
    /// Optional accounting period identifier to which this entry belongs.
    /// Example: links to monthly/quarterly accounting periods for reporting.
    /// </summary>
    public DefaultIdType? PeriodId { get; private set; }

    /// <summary>
    /// Original amount for reference or control purposes; not used for balancing.
    /// Example: 1500.00 for the original invoice amount before adjustments.
    /// </summary>
    public decimal OriginalAmount { get; private set; }


    private readonly List<JournalEntryLine> _lines = new();
    /// <summary>
    /// Collection of journal entry lines, each representing a debit or credit to a specific account.
    /// </summary>
    public IReadOnlyCollection<JournalEntryLine> Lines => _lines.AsReadOnly();

    private JournalEntry()
    {
        // EF Core requires a parameterless constructor for entity instantiation
        ReferenceNumber = string.Empty;
        Source = string.Empty;
        Status = "Pending";
    }

    private JournalEntry(DateTime date, string referenceNumber, string description, string source,
        DefaultIdType? periodId = null, decimal originalAmount = 0)
    {
        Date = date;
        ReferenceNumber = referenceNumber.Trim();
        Description = description.Trim();
        Source = source.Trim();
        IsPosted = false;
        PeriodId = periodId;
        OriginalAmount = originalAmount;
        Status = "Pending";

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
    /// Post the journal entry. Caller must verify balance before calling.
    /// </summary>
    public JournalEntry Post()
    {
        if (IsPosted)
            throw new JournalEntryAlreadyPostedException(Id);

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
    public void Approve(DefaultIdType approverId, string? approverName = null)
    {
        if (Status == "Approved")
            throw new InvalidOperationException("Journal entry already approved.");
        Status = "Approved";
        ApprovedBy = approverId;
        ApproverName = approverName?.Trim();
        ApprovedOn = DateTime.UtcNow;
        QueueDomainEvent(new JournalEntryApproved(Id, approverId.ToString(), ApprovedOn.Value));
    }

    /// <summary>
    /// Reject the journal entry, setting reviewer and timestamp.
    /// </summary>
    public void Reject(string rejectedBy)
    {
        if (Status == "Rejected")
            throw new InvalidOperationException("Journal entry already rejected.");
        Status = "Rejected";
        ApprovedBy = Guid.TryParse(rejectedBy, out var guidValue) ? guidValue : null;
        ApproverName = rejectedBy;
        ApprovedOn = DateTime.UtcNow;
        QueueDomainEvent(new JournalEntryRejected(Id, rejectedBy, ApprovedOn.Value));
    }

    /// <summary>
    /// Add a line to the journal entry.
    /// </summary>
    /// <param name="accountId">The chart of account identifier.</param>
    /// <param name="debitAmount">The debit amount.</param>
    /// <param name="creditAmount">The credit amount.</param>
    /// <param name="description">Optional line description.</param>
    /// <param name="reference">Optional line reference.</param>
    public void AddLine(DefaultIdType accountId, decimal debitAmount, decimal creditAmount, string? description = null, string? reference = null)
    {
        if (IsPosted)
            throw new JournalEntryCannotBeModifiedException(Id);

        var line = JournalEntryLine.Create(Id, accountId, debitAmount, creditAmount, description, reference);
        _lines.Add(line);
    }

    /// <summary>
    /// Calculate the total debit amount from all lines.
    /// </summary>
    public decimal GetTotalDebits() => Lines.Sum(l => l.DebitAmount);

    /// <summary>
    /// Calculate the total credit amount from all lines.
    /// </summary>
    public decimal GetTotalCredits() => Lines.Sum(l => l.CreditAmount);

    /// <summary>
    /// Calculate the difference between debits and credits.
    /// </summary>
    public decimal GetDifference() => GetTotalDebits() - GetTotalCredits();

    /// <summary>
    /// Check if the journal entry is balanced (debits = credits within tolerance).
    /// </summary>
    /// <param name="tolerance">The tolerance for rounding errors (default 0.01).</param>
    public bool IsBalanced(decimal tolerance = 0.01m) => Math.Abs(GetDifference()) < tolerance;

    /// <summary>
    /// Validates that the journal entry is balanced, throws exception if not.
    /// </summary>
    /// <exception cref="JournalEntryNotBalancedException">Thrown when entry is not balanced.</exception>
    public void ValidateBalance()
    {
        if (!IsBalanced())
        {
            throw new JournalEntryNotBalancedException(Id);
        }
    }
}
