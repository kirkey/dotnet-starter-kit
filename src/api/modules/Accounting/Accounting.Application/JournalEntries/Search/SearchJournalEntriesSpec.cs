using Accounting.Application.JournalEntries.Dtos;
using Accounting.Domain;
using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;

namespace Accounting.Application.JournalEntries.Search;

public sealed class SearchJournalEntriesSpec : EntitiesByPaginationFilterSpec<JournalEntry, JournalEntryDto>
{
    public SearchJournalEntriesSpec(SearchJournalEntriesRequest request) : base(request)
    {
        Query
            .OrderBy(e => e.Name!, !request.HasOrderBy())
            .Where(e => e.Name!.Contains(request.Name!), !string.IsNullOrEmpty(request.Name))
            .Where(e => e.ReferenceNumber!.Contains(request.ReferenceNumber!), !string.IsNullOrEmpty(request.ReferenceNumber))
            .Where(e => e.IsPosted == request.IsPosted, request.IsPosted.HasValue);
    }
}


