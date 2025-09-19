using Accounting.Application.JournalEntries.Responses;

namespace Accounting.Application.JournalEntries.Get;

public class GetJournalEntryQuery(DefaultIdType id) : IRequest<JournalEntryResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
