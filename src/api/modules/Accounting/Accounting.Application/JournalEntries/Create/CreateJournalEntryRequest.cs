namespace Accounting.Application.JournalEntries.Create;

public record CreateJournalEntryRequest(
    DateTime Date,
    string ReferenceNumber,
    string Description,
    string Source,
    DefaultIdType? PeriodId = null,
    decimal OriginalAmount = 0) : IRequest<DefaultIdType>;
