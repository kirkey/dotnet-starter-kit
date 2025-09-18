namespace Accounting.Application.PostingBatches.Dtos;

public class PostingBatchDto
{
    public DefaultIdType Id { get; set; }
    public string BatchNumber { get; set; } = default!;
    public DateTime BatchDate { get; set; }
    public string Status { get; set; } = default!;
    public string? Description { get; set; }
    public DefaultIdType? PeriodId { get; set; }
    public string ApprovalStatus { get; set; } = default!;
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public List<DefaultIdType> JournalEntryIds { get; set; } = new();
}