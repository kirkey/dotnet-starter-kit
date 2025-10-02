namespace Accounting.Application.JournalEntries.Reject;

/// <summary>
/// Command to reject a Journal Entry.
/// </summary>
public sealed record RejectJournalEntryCommand(
    DefaultIdType JournalEntryId,
    string RejectedBy,
    string? RejectionReason = null
) : IRequest<DefaultIdType>;
