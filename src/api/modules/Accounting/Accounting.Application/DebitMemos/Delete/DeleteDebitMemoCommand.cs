namespace Accounting.Application.DebitMemos.Delete;

/// <summary>
/// Command to delete a debit memo (draft status only).
/// </summary>
public sealed record DeleteDebitMemoCommand(DefaultIdType Id) : IRequest<DefaultIdType>;
