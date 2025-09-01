using MediatR;

namespace Accounting.Application.JournalEntries.Update;

public record UpdateJournalEntryRequest(
    DefaultIdType Id,
    string? ReferenceNumber = null,
    DateTime? Date = null,
    string? Source = null,
    DefaultIdType? PeriodId = null,
    decimal? OriginalAmount = null,
    string? Description = null,
    string? Notes = null) : IRequest<DefaultIdType>;
