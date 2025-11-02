namespace Accounting.Application.JournalEntries.Lines.Specs;

/// <summary>
/// Specification to get all journal entry lines for a specific journal entry.
/// </summary>
public sealed class JournalEntryLinesByJournalEntryIdSpec : Specification<JournalEntryLine>
{
    public JournalEntryLinesByJournalEntryIdSpec(DefaultIdType journalEntryId)
    {
        Query.Where(x => x.JournalEntryId == journalEntryId);
    }
}
