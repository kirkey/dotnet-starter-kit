namespace Accounting.Application.JournalEntries.Lines.Specs;

/// <summary>
/// Specification to get a single journal entry line by ID.
/// Includes the Account navigation property to populate AccountCode and AccountName.
/// </summary>
public sealed class GetJournalEntryLineSpec : Specification<JournalEntryLine>
{
    public GetJournalEntryLineSpec(DefaultIdType id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Account);
    }
}

