namespace Accounting.Application.JournalEntries.Lines.Search;

using Responses;

/// <summary>
/// Query to search journal entry lines by journal entry ID.
/// </summary>
/// <param name="JournalEntryId">The parent journal entry identifier.</param>
public sealed record SearchJournalEntryLinesByJournalEntryIdQuery(DefaultIdType JournalEntryId) 
    : IRequest<List<JournalEntryLineResponse>>;

