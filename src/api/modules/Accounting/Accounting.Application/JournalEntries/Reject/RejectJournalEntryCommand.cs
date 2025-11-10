namespace Accounting.Application.JournalEntries.Reject;

/// <summary>
/// Command to reject a Journal Entry.
/// The rejector is automatically determined from the current user session.
/// </summary>
public sealed record RejectJournalEntryCommand(
    DefaultIdType JournalEntryId,
    string? RejectionReason = null
) : IRequest<DefaultIdType>;
