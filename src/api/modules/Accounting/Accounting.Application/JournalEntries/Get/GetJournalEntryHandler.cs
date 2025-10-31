using Accounting.Application.JournalEntries.Responses;
using Accounting.Application.JournalEntries.Specs;

namespace Accounting.Application.JournalEntries.Get;

/// <summary>
/// Handler for getting a journal entry by ID.
/// Uses database-level projection for optimal performance with caching.
/// </summary>
public sealed class GetJournalEntryHandler(
    [FromKeyedServices("accounting:journals")] IReadRepository<JournalEntry> repository,
    ICacheService cache)
    : IRequestHandler<GetJournalEntryQuery, JournalEntryResponse>
{
    /// <summary>
    /// Handles the get journal entry query.
    /// </summary>
    /// <param name="request">The query containing the journal entry ID.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The journal entry response.</returns>
    /// <exception cref="JournalEntryNotFoundException">Thrown when journal entry is not found.</exception>
    public async Task<JournalEntryResponse> Handle(GetJournalEntryQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"journal:{request.Id}",
            async () =>
            {
                var spec = new GetJournalEntrySpec(request.Id);
                return await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
                    ?? throw new JournalEntryNotFoundException(request.Id);
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
