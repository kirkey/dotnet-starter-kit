using Accounting.Application.RecurringJournalEntries.Responses;

namespace Accounting.Application.RecurringJournalEntries.Search.v1;

/// <summary>
/// Handler for searching recurring journal entries with filters and pagination.
/// </summary>
public sealed class SearchRecurringJournalEntriesHandler(
    ILogger<SearchRecurringJournalEntriesHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<RecurringJournalEntry> repository)
    : IRequestHandler<SearchRecurringJournalEntriesRequest, PagedList<RecurringJournalEntryResponse>>
{
    public async Task<PagedList<RecurringJournalEntryResponse>> Handle(SearchRecurringJournalEntriesRequest request, CancellationToken cancellationToken)
    {
        var spec = new SearchRecurringJournalEntriesSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved {Count} of {Total} recurring journal entries", items.Count, totalCount);

        return new PagedList<RecurringJournalEntryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
