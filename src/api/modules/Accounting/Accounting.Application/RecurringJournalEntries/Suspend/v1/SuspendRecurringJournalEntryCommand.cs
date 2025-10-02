namespace Accounting.Application.RecurringJournalEntries.Suspend.v1;

public sealed class SuspendRecurringJournalEntryCommand : IRequest
{
    public DefaultIdType Id { get; set; }
    public string? Reason { get; set; }
}
