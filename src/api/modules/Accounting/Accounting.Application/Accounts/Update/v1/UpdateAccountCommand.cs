using Accounting.Domain;
using Accounting.Domain.Enums;
using FSH.Framework.Core.Dto;
using MediatR;

namespace Accounting.Application.Accounts.Update.v1;
public sealed record UpdateAccountCommand(
    Category Category,
    TransactionType TransactionType,
    string ParentCode,
    string Code,
    decimal Balance = 0)
    : BaseDto, IRequest<UpdateAccountResponse>;
