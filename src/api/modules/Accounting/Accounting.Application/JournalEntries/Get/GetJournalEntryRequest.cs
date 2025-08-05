using MediatR;
using Accounting.Application.JournalEntries.Dtos;

namespace Accounting.Application.JournalEntries.Get;

public record GetJournalEntryRequest(DefaultIdType Id) : IRequest<JournalEntryDto>;
