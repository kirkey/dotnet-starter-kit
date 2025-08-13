using Accounting.Application.JournalEntries.Dtos;
using Accounting.Domain;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.JournalEntries.Search;

public sealed class SearchJournalEntriesHandler(
    [FromKeyedServices("accounting:journals")] IReadRepository<JournalEntry> repository)
    : IRequestHandler<SearchJournalEntriesRequest, PagedList<JournalEntryDto>>
{
    public async Task<PagedList<JournalEntryDto>> Handle(SearchJournalEntriesRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchJournalEntriesSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<JournalEntryDto>(list, request.PageNumber, request.PageSize, totalCount);
    }
}


