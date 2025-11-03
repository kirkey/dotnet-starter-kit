namespace Accounting.Application.PostingBatches.Responses;

/// <summary>
/// Response model representing a posting batch entity.
/// Contains batch processing information including status, approval details, and associated journal entries.
/// </summary>
public class PostingBatchResponse
{
    /// <summary>
    /// Unique identifier for the posting batch.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// Unique batch number for reference and tracking.
    /// </summary>
    public string BatchNumber { get; set; } = null!;
    
    /// <summary>
    /// Date when the batch was created or processed.
    /// </summary>
    public DateTime BatchDate { get; set; }
    
    /// <summary>
    /// Current status of the batch (e.g., "Draft", "Posted", "Approved").
    /// </summary>
    public string Status { get; set; } = null!;
    
    /// <summary>
    /// Description or purpose of the posting batch.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Accounting period identifier for this batch.
    /// </summary>
    public DefaultIdType? PeriodId { get; set; }
    
    /// <summary>
    /// Approval status of the batch (e.g., "Pending", "Approved", "Rejected").
    /// </summary>
    public string ApprovalStatus { get; set; } = null!;
    
    /// <summary>
    /// Name of the person who approved the batch.
    /// </summary>
    public string? ApprovedBy { get; set; }
    
    /// <summary>
    /// Date when the batch was approved.
    /// </summary>
    public DateTime? ApprovedDate { get; set; }
    
    /// <summary>
    /// List of journal entry identifiers included in this batch.
    /// </summary>
    public List<DefaultIdType> JournalEntryIds { get; set; } = new();
}
