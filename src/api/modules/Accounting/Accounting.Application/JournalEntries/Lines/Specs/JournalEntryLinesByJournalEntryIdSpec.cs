namespace Accounting.Application.JournalEntries.Lines.Specs;

/// <summary>
/// Specification to get all journal entry lines for a specific journal entry.
/// Includes the Account navigation property to populate AccountCode and AccountName.
/// </summary>
public sealed class JournalEntryLinesByJournalEntryIdSpec : Specification<JournalEntryLine>
{
    public JournalEntryLinesByJournalEntryIdSpec(DefaultIdType journalEntryId)
    {
        Query
            .Where(x => x.JournalEntryId == journalEntryId)
            .Include(x => x.Account);
    }
}
