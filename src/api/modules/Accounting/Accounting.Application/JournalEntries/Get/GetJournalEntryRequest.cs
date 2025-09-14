using Accounting.Application.JournalEntries.Dtos;

namespace Accounting.Application.JournalEntries.Get;

public class GetJournalEntryRequest(DefaultIdType id) : IRequest<JournalEntryDto>
{
    public DefaultIdType Id { get; set; } = id;
}
