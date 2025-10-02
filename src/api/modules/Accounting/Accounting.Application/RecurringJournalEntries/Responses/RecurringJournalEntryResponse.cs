namespace Accounting.Application.RecurringJournalEntries.Responses;

public class RecurringJournalEntryResponse
{
    public DefaultIdType Id { get; set; }
    public string TemplateCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public int? CustomIntervalDays { get; set; }
    public decimal Amount { get; set; }
    public DefaultIdType DebitAccountId { get; set; }
    public DefaultIdType CreditAccountId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime NextRunDate { get; set; }
    public DateTime? LastGeneratedDate { get; set; }
    public int GeneratedCount { get; set; }
    public bool IsActive { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? Memo { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
    public Guid? LastModifiedBy { get; set; }
}
