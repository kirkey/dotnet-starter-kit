using Accounting.Application.RecurringJournalEntries.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.RecurringJournalEntries.Specs;

/// <summary>
/// Specification to retrieve a recurring journal entry by ID projected to <see cref="RecurringJournalEntryResponse"/>.
/// Performs database-level projection for optimal performance.
/// </summary>
public sealed class GetRecurringJournalEntrySpec : Specification<RecurringJournalEntry, RecurringJournalEntryResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetRecurringJournalEntrySpec"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the recurring journal entry to retrieve.</param>
    public GetRecurringJournalEntrySpec(DefaultIdType id)
    {
        Query.Where(e => e.Id == id);
    }
}

