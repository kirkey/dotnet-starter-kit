namespace Accounting.Application.RecurringJournalEntries.Create.v1;

/// <summary>
/// Command to create a new recurring journal entry template.
/// </summary>
public class CreateRecurringJournalEntryCommand : BaseRequest, IRequest<DefaultIdType>
{
    public string TemplateCode { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public int? CustomIntervalDays { get; set; }
    public decimal Amount { get; set; }
    public DefaultIdType DebitAccountId { get; set; }
    public DefaultIdType CreditAccountId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Memo { get; set; }
}
