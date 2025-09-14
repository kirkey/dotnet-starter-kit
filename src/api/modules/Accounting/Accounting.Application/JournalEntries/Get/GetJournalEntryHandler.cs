using Accounting.Application.JournalEntries.Dtos;
using JournalEntryNotFoundException = Accounting.Application.JournalEntries.Exceptions.JournalEntryNotFoundException;

namespace Accounting.Application.JournalEntries.Get;

public sealed class GetJournalEntryHandler(
    [FromKeyedServices("accounting:journals")] IReadRepository<JournalEntry> repository,
    ICacheService cache)
    : IRequestHandler<GetJournalEntryRequest, JournalEntryDto>
{
    public async Task<JournalEntryDto> Handle(GetJournalEntryRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"journal:{request.Id}",
            async () =>
            {
                var entry = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (entry == null) throw new JournalEntryNotFoundException(request.Id.ToString());
                return entry.Adapt<JournalEntryDto>();
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
