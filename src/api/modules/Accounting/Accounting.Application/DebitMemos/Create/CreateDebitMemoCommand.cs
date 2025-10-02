namespace Accounting.Application.DebitMemos.Create;

/// <summary>
/// Command to create a new debit memo.
/// </summary>
public sealed record CreateDebitMemoCommand(
    string MemoNumber,
    DateTime MemoDate,
    decimal Amount,
    string ReferenceType,
    DefaultIdType ReferenceId,
    DefaultIdType? OriginalDocumentId = null,
    string? Reason = null,
    string? Description = null,
    string? Notes = null
) : IRequest<DefaultIdType>;
