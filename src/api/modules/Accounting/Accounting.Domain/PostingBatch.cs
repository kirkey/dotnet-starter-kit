namespace Accounting.Domain;

/// <summary>
/// Groups multiple journal entries for approval and posting as a batch.
/// </summary>
/// <remarks>
/// Supports an approval workflow and a posting lifecycle. Defaults: <see cref="Status"/> starts as "Draft",
/// <see cref="ApprovalStatus"/> starts as "Pending". Only approved draft batches can be posted.
/// </remarks>
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
