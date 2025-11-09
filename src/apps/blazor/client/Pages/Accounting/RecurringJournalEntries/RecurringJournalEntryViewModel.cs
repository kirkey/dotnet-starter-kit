namespace FSH.Starter.Blazor.Client.Pages.Accounting.RecurringJournalEntries;

/// <summary>
/// ViewModel for creating or editing recurring journal entries.
/// </summary>
public sealed class RecurringJournalEntryViewModel
{
    public DefaultIdType? Id { get; set; }
    public string? TemplateCode { get; set; }
    public string? Description { get; set; }
    public string Frequency { get; set; } = "Monthly";
    public int? CustomIntervalDays { get; set; }
    public decimal Amount { get; set; }
    public DefaultIdType? DebitAccountId { get; set; }
    public DefaultIdType? CreditAccountId { get; set; }
    public DateTime? StartDate { get; set; } = DateTime.Today;
    public DateTime? EndDate { get; set; }
    public DateTime? NextRunDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string Status { get; set; } = "Draft";
    public string? Memo { get; set; }
    public string? Notes { get; set; }
}

