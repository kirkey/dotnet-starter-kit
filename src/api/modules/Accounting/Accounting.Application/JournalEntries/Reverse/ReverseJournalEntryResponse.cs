namespace Accounting.Application.JournalEntries.Reverse;

/// <summary>
/// Response containing the ID of the newly created reversing journal entry.
/// </summary>
public sealed record ReverseJournalEntryResponse(DefaultIdType ReversingEntryId);
