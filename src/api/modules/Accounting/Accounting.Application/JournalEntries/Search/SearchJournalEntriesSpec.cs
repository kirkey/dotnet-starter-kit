using Accounting.Application.JournalEntries.Responses;

namespace Accounting.Application.JournalEntries.Search;

/// <summary>
/// Specification for searching journal entries with filtering and pagination.
/// Projects results to <see cref="JournalEntryResponse"/> including lines and approval information.
/// </summary>
public sealed class SearchJournalEntriesSpec : EntitiesByPaginationFilterSpec<JournalEntry, JournalEntryResponse>
{
    public SearchJournalEntriesSpec(SearchJournalEntriesRequest request) : base(request)
    {
        Query
            .Include(e => e.Lines)
                .ThenInclude(l => l.Account)
            .OrderByDescending(e => e.Date, !request.HasOrderBy())
            .Where(e => e.ReferenceNumber!.Contains(request.ReferenceNumber!), !string.IsNullOrEmpty(request.ReferenceNumber))
            .Where(e => e.Source!.Contains(request.Source!), !string.IsNullOrEmpty(request.Source))
            .Where(e => e.Date >= request.FromDate, request.FromDate.HasValue)
            .Where(e => e.Date <= request.ToDate, request.ToDate.HasValue)
            .Where(e => e.IsPosted == request.IsPosted, request.IsPosted.HasValue)
            .Where(e => e.Status == request.ApprovalStatus!, !string.IsNullOrEmpty(request.ApprovalStatus))
            .Where(e => e.PeriodId == request.PeriodId, request.PeriodId.HasValue);
    }
}
