using Accounting.Application.RecurringJournalEntries.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.RecurringJournalEntries.Search.v1;

public sealed class SearchRecurringJournalEntriesHandler(
    IReadRepository<RecurringJournalEntry> repository)
    : IRequestHandler<SearchRecurringJournalEntriesCommand, PagedList<RecurringJournalEntryResponse>>
{
    public async Task<PagedList<RecurringJournalEntryResponse>> Handle(SearchRecurringJournalEntriesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchRecurringJournalEntriesSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        return new PagedList<RecurringJournalEntryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
