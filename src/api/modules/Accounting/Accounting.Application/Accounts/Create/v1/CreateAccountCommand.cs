using System.ComponentModel;
using Accounting.Domain.Enums;
using MediatR;

namespace Accounting.Application.Accounts.Create.v1;
public sealed record CreateAccountCommand(
    [property: DefaultValue("")] Category Category,
    [property: DefaultValue("")] TransactionType TransactionType,
    [property: DefaultValue("")] string ParentCode,
    [property: DefaultValue("")] string Code,
    [property: DefaultValue("")] string Name,
    [property: DefaultValue("0.00")] decimal? Balance = 0,
    string? Description = null,
    string? Notes = null)
    : IRequest<CreateAccountResponse>;
