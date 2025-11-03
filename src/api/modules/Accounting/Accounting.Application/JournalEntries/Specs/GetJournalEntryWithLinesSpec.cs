namespace Accounting.Application.JournalEntries.Specs;

/// <summary>
/// Specification to retrieve a journal entry by ID including its lines.
/// Used when the full aggregate with lines is needed (e.g., for posting, reversing).
/// </summary>
public sealed class GetJournalEntryWithLinesSpec : Specification<JournalEntry>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetJournalEntryWithLinesSpec"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the journal entry to retrieve.</param>
    public GetJournalEntryWithLinesSpec(DefaultIdType id)
    {
        Query
            .Where(j => j.Id == id)
            .Include(j => j.Lines);
    }
}

