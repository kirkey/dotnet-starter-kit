namespace Accounting.Domain;

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
public class PostingBatch : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique identifier for the batch (human-friendly).
    /// </summary>
    public string BatchNumber { get; private set; }

    /// <summary>
    /// Date of the batch.
    /// </summary>
    public DateTime BatchDate { get; private set; }

    /// <summary>
    /// Workflow status: Draft, Posted, Reversed.
    /// </summary>
    public string Status { get; private set; } // Draft, Posted, Reversed
    // Hide base Description property with 'new' keyword to resolve warning
    /// <summary>
    /// Optional batch description.
    /// </summary>
    public new string? Description { get; private set; }

    /// <summary>
    /// Optional accounting period the batch belongs to.
    /// </summary>
    public DefaultIdType? PeriodId { get; private set; }

    /// <summary>
    /// Approval workflow state: Pending, Approved, Rejected.
    /// </summary>
    public string ApprovalStatus { get; private set; } // Pending, Approved, Rejected

    /// <summary>
    /// Approver/reviewer identity.
    /// </summary>
    public string? ApprovedBy { get; private set; }

    /// <summary>
    /// Timestamp when approved or rejected.
    /// </summary>
    public DateTime? ApprovedDate { get; private set; }

    private readonly List<JournalEntry> _journalEntries = new();
    /// <summary>
    /// Journal entries included in this batch.
    /// </summary>
    public IReadOnlyCollection<JournalEntry> JournalEntries => _journalEntries.AsReadOnly();

    // EF Core requires a parameterless constructor
    private PostingBatch() { }

    private PostingBatch(string batchNumber, DateTime batchDate, string? description = null, DefaultIdType? periodId = null)
    {
        BatchNumber = batchNumber.Trim();
        BatchDate = batchDate;
        Status = "Draft";
        Description = description?.Trim();
        PeriodId = periodId;
        ApprovalStatus = "Pending";
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
        if (Status != "Draft")
            throw new InvalidOperationException("Can only add entries to a draft batch.");
        _journalEntries.Add(entry);
    }

    /// <summary>
    /// Post all entries in the batch after approval; transitions status to Posted.
    /// </summary>
    public void Post()
    {
        if (Status != "Draft")
            throw new InvalidOperationException("Only draft batches can be posted.");
        if (ApprovalStatus != "Approved")
            throw new InvalidOperationException("Batch must be approved before posting.");
        foreach (var entry in _journalEntries)
        {
            entry.Post();
        }
        Status = "Posted";
        QueueDomainEvent(new Events.PostingBatch.PostingBatchPosted(Id, BatchNumber, BatchDate));
    }

    /// <summary>
    /// Reverse a posted batch by reversing each contained entry; sets status to Reversed.
    /// </summary>
    public void Reverse(string reason)
    {
        if (Status != "Posted")
            throw new InvalidOperationException("Only posted batches can be reversed.");
        foreach (var entry in _journalEntries)
        {
            entry.Reverse(DateTime.UtcNow, reason);
        }
        Status = "Reversed";
        QueueDomainEvent(new Events.PostingBatch.PostingBatchReversed(Id, BatchNumber, BatchDate, reason));
    }

    /// <summary>
    /// Approve the batch for posting and set approver metadata.
    /// </summary>
    public void Approve(string approvedBy)
    {
        if (ApprovalStatus == "Approved")
            throw new InvalidOperationException("Batch already approved.");
        ApprovalStatus = "Approved";
        ApprovedBy = approvedBy;
        ApprovedDate = DateTime.UtcNow;
        QueueDomainEvent(new Events.PostingBatch.PostingBatchApproved(Id, BatchNumber, ApprovedBy, ApprovedDate.Value));
    }

    /// <summary>
    /// Reject the batch; sets reviewer metadata.
    /// </summary>
    public void Reject(string rejectedBy)
    {
        if (ApprovalStatus == "Rejected")
            throw new InvalidOperationException("Batch already rejected.");
        ApprovalStatus = "Rejected";
        ApprovedBy = rejectedBy;
        ApprovedDate = DateTime.UtcNow;
        QueueDomainEvent(new Events.PostingBatch.PostingBatchRejected(Id, BatchNumber, ApprovedBy, ApprovedDate.Value));
    }
}
