namespace Accounting.Application.CreditMemos.Delete;

/// <summary>
/// Command to delete a credit memo (draft status only).
/// </summary>
public sealed record DeleteCreditMemoCommand(DefaultIdType Id) : IRequest<DefaultIdType>;
