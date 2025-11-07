namespace Accounting.Application.PostingBatches.Get.v1;

/// <summary>
/// Response containing posting batch details.
/// </summary>
public sealed record PostingBatchGetResponse
{
    public DefaultIdType Id { get; init; }
    public string BatchNumber { get; init; } = string.Empty;
    public DateTime BatchDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public DefaultIdType? PeriodId { get; init; }
    public string ApprovalStatus { get; init; } = string.Empty;
    public string? ApprovedBy { get; init; }
    public DateTime? ApprovedDate { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public int JournalEntryCount { get; init; }
    public DateTime CreatedOn { get; init; }
    public string? CreatedByUserName { get; init; }
}

