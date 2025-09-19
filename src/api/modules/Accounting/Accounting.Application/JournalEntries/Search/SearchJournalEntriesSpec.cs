using Accounting.Application.JournalEntries.Dtos;

namespace Accounting.Application.JournalEntries.Search;

public sealed class SearchJournalEntriesSpec : EntitiesByPaginationFilterSpec<JournalEntry, JournalEntryDto>
{
    public SearchJournalEntriesSpec(SearchJournalEntriesQuery request) : base(request)
    {
        Query
            .OrderBy(e => e.Name!, !request.HasOrderBy())
            .Where(e => e.Name!.Contains(request.Name!), !string.IsNullOrEmpty(request.Name))
            .Where(e => e.ReferenceNumber!.Contains(request.ReferenceNumber!), !string.IsNullOrEmpty(request.ReferenceNumber))
            .Where(e => e.IsPosted == request.IsPosted);
    }
}
