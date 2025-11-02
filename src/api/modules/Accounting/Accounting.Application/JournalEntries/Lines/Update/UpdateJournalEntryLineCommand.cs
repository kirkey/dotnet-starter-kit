namespace Accounting.Application.JournalEntries.Lines.Update;

/// <summary>
/// Command to update an existing journal entry line.
/// </summary>
/// <param name="Id">The journal entry line identifier to update.</param>
/// <param name="DebitAmount">Optional updated debit amount.</param>
/// <param name="CreditAmount">Optional updated credit amount.</param>
/// <param name="Memo">Optional updated memo/description.</param>
/// <param name="Reference">Optional updated reference.</param>
public sealed record UpdateJournalEntryLineCommand(
    DefaultIdType Id,
    decimal? DebitAmount,
    decimal? CreditAmount,
    string? Memo,
    string? Reference
) : IRequest;
