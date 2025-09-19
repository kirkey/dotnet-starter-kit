using Accounting.Application.JournalEntries.Responses;

namespace Accounting.Application.JournalEntries.Get;

public sealed class GetJournalEntryHandler(
    [FromKeyedServices("accounting:journals")] IReadRepository<JournalEntry> repository,
    ICacheService cache)
    : IRequestHandler<GetJournalEntryQuery, JournalEntryResponse>
{
    public async Task<JournalEntryResponse> Handle(GetJournalEntryQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"journal:{request.Id}",
            async () =>
            {
                var entry = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (entry == null) throw new JournalEntryNotFoundException(request.Id);
                return entry.Adapt<JournalEntryResponse>();
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
