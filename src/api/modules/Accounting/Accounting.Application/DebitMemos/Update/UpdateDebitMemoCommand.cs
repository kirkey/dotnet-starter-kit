namespace Accounting.Application.DebitMemos.Update;

/// <summary>
/// Command to update a debit memo.
/// </summary>
public sealed record UpdateDebitMemoCommand(
    DefaultIdType Id,
    DateTime? MemoDate = null,
    decimal? Amount = null,
    string? Reason = null,
    string? Description = null,
    string? Notes = null
) : IRequest<DefaultIdType>;
