namespace Accounting.Application.JournalEntries.Post;

/// <summary>
/// Command to post a Journal Entry to the General Ledger.
/// Validates that the entry is balanced before posting.
/// </summary>
public sealed record PostJournalEntryCommand(
    DefaultIdType JournalEntryId
) : IRequest<DefaultIdType>;
