namespace Accounting.Application.JournalEntries.Lines.Create;

/// <summary>
/// Command to create a new journal entry line for a specific journal entry and account.
/// </summary>
/// <param name="JournalEntryId">The parent journal entry identifier.</param>
/// <param name="AccountId">The chart of account identifier this line applies to.</param>
/// <param name="DebitAmount">The debit amount (must be non-negative, either debit or credit should be non-zero).</param>
/// <param name="CreditAmount">The credit amount (must be non-negative, either debit or credit should be non-zero).</param>
/// <param name="Memo">Optional memo/description up to 500 characters.</param>
/// <param name="Reference">Optional reference up to 100 characters.</param>
public sealed record CreateJournalEntryLineCommand(
    DefaultIdType JournalEntryId,
    DefaultIdType AccountId,
    decimal DebitAmount,
    decimal CreditAmount,
    string? Memo,
    string? Reference
) : IRequest<DefaultIdType>;
