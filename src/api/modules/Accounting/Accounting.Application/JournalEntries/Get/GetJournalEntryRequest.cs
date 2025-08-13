using MediatR;
using Accounting.Application.JournalEntries.Dtos;

namespace Accounting.Application.JournalEntries.Get;

public class GetJournalEntryRequest : IRequest<JournalEntryDto>
{
    public DefaultIdType Id { get; set; }

    public GetJournalEntryRequest(DefaultIdType id)
    {
        Id = id;
    }
}
