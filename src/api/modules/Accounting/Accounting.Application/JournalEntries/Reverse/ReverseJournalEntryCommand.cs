namespace Accounting.Application.JournalEntries.Reverse;

/// <summary>
/// Command to reverse a posted Journal Entry.
/// Creates a new reversing entry with opposite debit/credit amounts.
/// </summary>
public sealed record ReverseJournalEntryCommand(
    DefaultIdType JournalEntryId,
    DateTime ReversalDate,
    string ReversalReason
) : IRequest<DefaultIdType>;
