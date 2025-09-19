using Accounting.Application.JournalEntries.Responses;

namespace Accounting.Application.JournalEntries.Search;

public sealed class SearchJournalEntriesSpec : EntitiesByPaginationFilterSpec<JournalEntry, JournalEntryResponse>
{
    public SearchJournalEntriesSpec(SearchJournalEntriesQuery request) : base(request)
    {
        Query
            .OrderBy(e => e.Date, !request.HasOrderBy())
            .Where(e => e.ReferenceNumber!.Contains(request.ReferenceNumber!), !string.IsNullOrEmpty(request.ReferenceNumber))
            .Where(e => e.Source!.Contains(request.Source!), !string.IsNullOrEmpty(request.Source))
            .Where(e => e.Date >= request.FromDate, request.FromDate.HasValue)
            .Where(e => e.Date <= request.ToDate, request.ToDate.HasValue)
            .Where(e => e.IsPosted == request.IsPosted, request.IsPosted.HasValue);
    }
}
