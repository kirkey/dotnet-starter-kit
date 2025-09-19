using Accounting.Application.JournalEntries.Dtos;

namespace Accounting.Application.JournalEntries.Get;

public class GetJournalEntryQuery(DefaultIdType id) : IRequest<JournalEntryDto>
{
    public DefaultIdType Id { get; set; } = id;
}
