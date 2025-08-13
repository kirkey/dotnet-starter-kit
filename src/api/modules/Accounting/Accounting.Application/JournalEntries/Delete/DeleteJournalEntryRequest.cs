using MediatR;

namespace Accounting.Application.JournalEntries.Delete;

public class DeleteJournalEntryRequest : IRequest
{
    public DefaultIdType Id { get; set; }

    public DeleteJournalEntryRequest(DefaultIdType id)
    {
        Id = id;
    }
}
