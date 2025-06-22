using MediatR;

namespace Accounting.Application.Payees.Update.v1;
public sealed record PayeeUpdateCommand(
    DefaultIdType Id,
    string PayeeCode,
    string Name,
    string? Address,
    string? ExpenseAccountCode,
    string? ExpenseAccountName,
    string? Tin,
    string? Description,
    string? Notes) : IRequest<PayeeUpdateResponse>;
