namespace Accounting.Domain.Entities;

/// <summary>
/// Groups multiple journal entries for approval and posting as a batch to ensure transaction integrity and workflow control.
/// </summary>
/// <remarks>
/// Use cases:
/// - Group related journal entries for batch processing and approval workflows.
/// - Ensure transaction integrity by posting multiple entries atomically.
/// - Support month-end closing procedures with organized batch posting.
/// - Enable supervisor approval of journal entries before posting to general ledger.
/// - Track posting batches for audit trails and error resolution.
/// - Support batch reversal capabilities for correcting posting errors.
/// - Facilitate reconciliation by grouping entries from the same source system.
/// - Enable automated posting of recurring entries and system-generated transactions.
/// 
/// Default values:
/// - BatchNumber: required unique identifier (example: "BATCH-2025-09-001", "MONTH-END-SEP-2025")
/// - BatchDate: required effective date for the batch (example: 2025-09-30 for month-end entries)
/// - Status: "Draft" (new batches start as draft until posted)
/// - ApprovalStatus: "Pending" (awaiting approval before posting)
/// - Description: optional batch description (example: "September 2025 month-end accruals")
/// - ApprovedBy: null (set when batch is approved)
/// - ApprovedDate: null (set when approval occurs)
/// - PostedBy: null (set when batch is posted)
/// - PostedDate: null (set when posting occurs)
/// - TotalDebitAmount: calculated from journal entries in batch
/// - TotalCreditAmount: calculated from journal entries in batch
/// 
/// Business rules:
/// - BatchNumber must be unique within the system
/// - Only approved batches can be posted to the general ledger
/// - Batch must be balanced (total debits = total credits) before posting
/// - Cannot modify batch contents after posting
/// - Reversal creates a new batch with opposite entries
/// - Approval requires proper authorization levels
/// - Posted batches cannot be deleted, only reversed
/// - All journal entries in batch must reference the same accounting period
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.PostingBatch.PostingBatchCreated"/>
/// <seealso cref="Accounting.Domain.Events.PostingBatch.PostingBatchApproved"/>
/// <seealso cref="Accounting.Domain.Events.PostingBatch.PostingBatchPosted"/>
/// <seealso cref="Accounting.Domain.Events.PostingBatch.PostingBatchReversed"/>
/// <seealso cref="Accounting.Domain.Events.PostingBatch.PostingBatchRejected"/>
public class PostingBatch : AuditableEntityWithApproval, IAggregateRoot
{
    /// <summary>
    /// Unique identifier for the batch (human-friendly).
    /// </summary>
    public string BatchNumber { get; private set; }

    /// <summary>
    /// Date of the batch.
    /// </summary>
    public DateTime BatchDate { get; private set; }


    // Description property inherited from AuditableEntity base class

    /// <summary>
    /// Optional accounting period the batch belongs to.
    /// </summary>
    public DefaultIdType? PeriodId { get; private set; }


    /// <summary>
    /// Date when the batch is posted to general ledger (effective date).
    /// </summary>
    public DateTime PostingDate { get; private set; }

    /// <summary>
    /// User who posted the batch to the general ledger.
    /// </summary>
    public string? PostedBy { get; private set; }

    /// <summary>
    /// Timestamp when the batch was posted.
    /// </summary>
    public DateTime? PostedOn { get; private set; }

    /// <summary>
    /// User who reversed the batch.
    /// </summary>
    public string? ReversedBy { get; private set; }

    /// <summary>
    /// Timestamp when the batch was reversed.
    /// </summary>
    public DateTime? ReversedOn { get; private set; }

    /// <summary>
    /// Total debit amount across all journal entries (for validation).
    /// </summary>
    public decimal TotalDebits { get; private set; }

    /// <summary>
    /// Total credit amount across all journal entries (for validation).
    /// </summary>
    public decimal TotalCredits { get; private set; }

    /// <summary>
    /// Number of journal entries included in this batch.
    /// </summary>
    public int EntryCount { get; private set; }

    private readonly List<JournalEntry> _journalEntries = new();
    /// <summary>
    /// Journal entries included in this batch.
    /// </summary>
    public IReadOnlyCollection<JournalEntry> JournalEntries => _journalEntries.AsReadOnly();

    // EF Core requires a parameterless constructor
    private PostingBatch() 
    { 
        BatchNumber = string.Empty;
        Status = "Pending";
    }

    private PostingBatch(string batchNumber, DateTime batchDate, string? description = null, DefaultIdType? periodId = null)
    {
        BatchNumber = batchNumber.Trim();
        BatchDate = batchDate;
        PostingDate = batchDate; // Default posting date to batch date
        Status = "Pending";
        Description = description?.Trim();
        PeriodId = periodId;
        TotalDebits = 0;
        TotalCredits = 0;
        EntryCount = 0;

        QueueDomainEvent(new Events.PostingBatch.PostingBatchCreated(Id, BatchNumber, BatchDate, Description));
    }

    /// <summary>
    /// Create a posting batch with initial status Draft and Pending approval.
    /// </summary>
    public static PostingBatch Create(string batchNumber, DateTime batchDate, string? description = null, DefaultIdType? periodId = null)
    {
        if (string.IsNullOrWhiteSpace(batchNumber))
            throw new ArgumentException("Batch number is required.");
        return new PostingBatch(batchNumber, batchDate, description, periodId);
    }

    /// <summary>
    /// Add a journal entry to a draft batch only.
    /// </summary>
    public void AddJournalEntry(JournalEntry entry)
    {
        if (Status != "Pending" && Status != "Draft")
            throw new InvalidOperationException("Can only add entries to a pending or draft batch.");
        _journalEntries.Add(entry);
        RecalculateTotals();
    }

    /// <summary>
    /// Recalculate batch totals from journal entries.
    /// </summary>
    private void RecalculateTotals()
    {
        EntryCount = _journalEntries.Count;
        TotalDebits = _journalEntries.Sum(e => e.GetTotalDebits());
        TotalCredits = _journalEntries.Sum(e => e.GetTotalCredits());
    }

    /// <summary>
    /// Post all entries in the batch after approval; transitions status to Posted.
    /// </summary>
    public void Post(string postedBy)
    {
        if (Status == "Posted")
            throw new InvalidOperationException("Batch is already posted.");
        if (Status != "Approved")
            throw new InvalidOperationException("Batch must be approved before posting.");
        
        RecalculateTotals();
        if (TotalDebits != TotalCredits)
            throw new InvalidOperationException($"Batch is not balanced. Debits: {TotalDebits}, Credits: {TotalCredits}");

        foreach (var entry in _journalEntries)
        {
            entry.Post();
        }
        
        Status = "Posted";
        PostedBy = postedBy;
        PostedOn = DateTime.UtcNow;
        
        QueueDomainEvent(new Events.PostingBatch.PostingBatchPosted(Id, BatchNumber, BatchDate));
    }

    /// <summary>
    /// Reverse a posted batch by reversing each contained entry; sets status to Reversed.
    /// </summary>
    public void Reverse(string reversedBy, string reason)
    {
        if (Status != "Posted")
            throw new InvalidOperationException("Only posted batches can be reversed.");
        
        foreach (var entry in _journalEntries)
        {
            entry.Reverse(DateTime.UtcNow, reason);
        }
        
        Status = "Reversed";
        ReversedBy = reversedBy;
        ReversedOn = DateTime.UtcNow;
        
        QueueDomainEvent(new Events.PostingBatch.PostingBatchReversed(Id, BatchNumber, BatchDate, reason));
    }

    /// <summary>
    /// Approve the batch for posting and set approver metadata.
    /// </summary>
    public void Approve(DefaultIdType approverId, string? approverName = null)
    {
        if (Status == "Approved")
            throw new InvalidOperationException("Batch already approved.");
        Status = "Approved";
        ApprovedBy = approverId;
        ApproverName = approverName?.Trim();
        ApprovedOn = DateTime.UtcNow;
        QueueDomainEvent(new Events.PostingBatch.PostingBatchApproved(Id, BatchNumber, approverId.ToString(), ApprovedOn.Value));
    }

    /// <summary>
    /// Reject the batch; sets reviewer metadata.
    /// </summary>
    public void Reject(string rejectedBy)
    {
        if (Status == "Rejected")
            throw new InvalidOperationException("Batch already rejected.");
        Status = "Rejected";
        ApprovedBy = Guid.TryParse(rejectedBy, out var guidValue) ? guidValue : null;
        ApproverName = rejectedBy;
        ApprovedOn = DateTime.UtcNow;
        QueueDomainEvent(new Events.PostingBatch.PostingBatchRejected(Id, BatchNumber, rejectedBy, ApprovedOn.Value));
    }
}
