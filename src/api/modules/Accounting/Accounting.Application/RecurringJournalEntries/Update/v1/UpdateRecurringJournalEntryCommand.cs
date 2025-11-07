namespace Accounting.Application.RecurringJournalEntries.Update.v1;

public sealed record UpdateRecurringJournalEntryCommand(
    DefaultIdType Id,
    string? Description = null,
    decimal? Amount = null,
    DateTime? EndDate = null,
    string? Memo = null,
    string? Notes = null
) : IRequest<DefaultIdType>;
