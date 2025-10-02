namespace Accounting.Application.RecurringJournalEntries.Reactivate.v1;

public sealed record ReactivateRecurringJournalEntryCommand(DefaultIdType Id) : IRequest;
