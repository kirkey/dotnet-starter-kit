using MediatR;

namespace Accounting.Application.JournalEntries.Create;

public record CreateJournalEntryRequest(
    DateTime Date,
    string ReferenceNumber,
    string Description,
    string Source,
    DefaultIdType? PeriodId = null,
    DefaultIdType? CurrencyId = null,
    decimal ExchangeRate = 1.0m,
    decimal OriginalAmount = 0) : IRequest<DefaultIdType>;
