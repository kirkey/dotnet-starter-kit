using Accounting.Application.JournalEntries.Responses;

namespace Accounting.Application.JournalEntries.Search;

/// <summary>
/// Handler for searching journal entries.
/// </summary>
public sealed class SearchJournalEntriesHandler(
    [FromKeyedServices("accounting:journals")] IReadRepository<JournalEntry> repository)
    : IRequestHandler<SearchJournalEntriesRequest, PagedList<JournalEntryResponse>>
{
    public async Task<PagedList<JournalEntryResponse>> Handle(SearchJournalEntriesRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchJournalEntriesSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<JournalEntryResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}

