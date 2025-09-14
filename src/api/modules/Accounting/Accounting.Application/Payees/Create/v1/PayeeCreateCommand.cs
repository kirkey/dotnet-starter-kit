namespace Accounting.Application.Payees.Create.v1;

public sealed record PayeeCreateCommand(
    string PayeeCode,
    string Name,
    string? Address,
    string? ExpenseAccountCode,
    string? ExpenseAccountName,
    string? Tin,
    string? Description,
    string? Notes) : IRequest<PayeeCreateResponse>;
