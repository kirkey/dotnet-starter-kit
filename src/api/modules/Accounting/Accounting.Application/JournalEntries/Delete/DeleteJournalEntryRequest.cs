using MediatR;

namespace Accounting.Application.JournalEntries.Delete;

public class DeleteJournalEntryRequest(DefaultIdType id) : IRequest
{
    public DefaultIdType Id { get; set; } = id;
}
