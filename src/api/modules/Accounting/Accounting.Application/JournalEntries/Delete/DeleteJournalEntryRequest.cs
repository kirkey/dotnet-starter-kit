using MediatR;

namespace Accounting.Application.JournalEntries.Delete;

public record DeleteJournalEntryRequest(DefaultIdType Id) : IRequest;
