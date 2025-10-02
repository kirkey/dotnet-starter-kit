namespace Accounting.Application.RecurringJournalEntries.Delete.v1;

public sealed record DeleteRecurringJournalEntryCommand(DefaultIdType Id) : IRequest;
