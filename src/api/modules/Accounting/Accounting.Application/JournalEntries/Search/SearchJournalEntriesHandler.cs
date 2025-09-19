

namespace Accounting.Application.JournalEntries.Search;

public sealed class SearchJournalEntriesHandler(
    [FromKeyedServices("accounting:journals")] IReadRepository<JournalEntry> repository)
    : IRequestHandler<SearchJournalEntriesQuery, PagedList<JournalEntryDto>>
{
    public async Task<PagedList<JournalEntryDto>> Handle(SearchJournalEntriesQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchJournalEntriesSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<JournalEntryDto>(list, request.PageNumber, request.PageSize, totalCount);
    }
}


