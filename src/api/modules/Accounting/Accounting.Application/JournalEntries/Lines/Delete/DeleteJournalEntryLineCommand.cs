namespace Accounting.Application.JournalEntries.Lines.Delete;

/// <summary>
/// Command to delete a journal entry line.
/// </summary>
/// <param name="Id">The journal entry line identifier to delete.</param>
public sealed record DeleteJournalEntryLineCommand(DefaultIdType Id) : IRequest;

