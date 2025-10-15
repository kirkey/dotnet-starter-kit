using Accounting.Application.JournalEntries.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.JournalEntries.Specs;

/// <summary>
/// Specification to retrieve a journal entry by ID projected to <see cref="JournalEntryResponse"/>.
/// Performs database-level projection for optimal performance.
/// </summary>
public sealed class GetJournalEntrySpec : Specification<JournalEntry, JournalEntryResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetJournalEntrySpec"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the journal entry to retrieve.</param>
    public GetJournalEntrySpec(DefaultIdType id)
    {
        Query.Where(j => j.Id == id);
    }
}

