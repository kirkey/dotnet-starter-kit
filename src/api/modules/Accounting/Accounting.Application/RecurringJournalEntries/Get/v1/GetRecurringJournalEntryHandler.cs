using Accounting.Application.RecurringJournalEntries.Responses;
using Accounting.Application.RecurringJournalEntries.Specs;
using Accounting.Domain.Entities;

namespace Accounting.Application.RecurringJournalEntries.Get.v1;

/// <summary>
/// Handler for getting a recurring journal entry by ID.
/// Uses database-level projection for optimal performance.
/// </summary>
public sealed class GetRecurringJournalEntryHandler(
    IReadRepository<RecurringJournalEntry> repository)
    : IRequestHandler<GetRecurringJournalEntryRequest, RecurringJournalEntryResponse>
{
    /// <summary>
    /// Handles the get recurring journal entry request.
    /// </summary>
    /// <param name="request">The request containing the recurring journal entry ID.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The recurring journal entry response.</returns>
    /// <exception cref="RecurringJournalEntryNotFoundException">Thrown when entry is not found.</exception>
    public async Task<RecurringJournalEntryResponse> Handle(GetRecurringJournalEntryRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetRecurringJournalEntrySpec(request.Id);
        return await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
            ?? throw new RecurringJournalEntryNotFoundException(request.Id);
    }
}
