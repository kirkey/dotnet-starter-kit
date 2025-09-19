namespace Accounting.Application.JournalEntries.Delete;

/// <summary>
/// Command to delete a JournalEntry. Deletion is prevented for posted entries.
/// </summary>
public sealed record DeleteJournalEntryCommand(DefaultIdType Id) : IRequest;
