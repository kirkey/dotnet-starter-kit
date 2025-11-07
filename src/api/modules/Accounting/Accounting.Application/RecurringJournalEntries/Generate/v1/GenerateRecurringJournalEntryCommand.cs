namespace Accounting.Application.RecurringJournalEntries.Generate.v1;

public sealed record GenerateRecurringJournalEntryCommand(DefaultIdType Id, DateTime? GenerateForDate = null) : IRequest<DefaultIdType>;

