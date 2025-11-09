namespace Accounting.Application.JournalEntries.Approve;

/// <summary>
/// Command to approve a Journal Entry.
/// The approver is automatically determined from the current user session.
/// </summary>
public sealed record ApproveJournalEntryCommand(
    DefaultIdType JournalEntryId
) : IRequest<DefaultIdType>;
