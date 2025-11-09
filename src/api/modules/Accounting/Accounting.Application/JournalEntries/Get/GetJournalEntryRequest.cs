using Accounting.Application.JournalEntries.Responses;

namespace Accounting.Application.JournalEntries.Get;

/// <summary>
/// Request to retrieve a journal entry by ID.
/// </summary>
public class GetJournalEntryRequest(DefaultIdType id) : IRequest<JournalEntryResponse>
{
    /// <summary>
    /// The ID of the journal entry to retrieve.
    /// </summary>
    public DefaultIdType Id { get; set; } = id;
}
