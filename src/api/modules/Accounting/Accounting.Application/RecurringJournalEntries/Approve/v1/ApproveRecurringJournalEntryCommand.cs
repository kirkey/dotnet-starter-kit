namespace Accounting.Application.RecurringJournalEntries.Approve.v1;

public sealed class ApproveRecurringJournalEntryCommand : IRequest
{
    public DefaultIdType Id { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
}
