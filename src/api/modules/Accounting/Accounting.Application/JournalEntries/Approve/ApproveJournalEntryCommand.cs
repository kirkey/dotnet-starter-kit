namespace Accounting.Application.JournalEntries.Approve;

/// <summary>
/// Command to approve a Journal Entry.
/// </summary>
public sealed record ApproveJournalEntryCommand(
    DefaultIdType JournalEntryId,
    string ApprovedBy
) : IRequest<DefaultIdType>;
