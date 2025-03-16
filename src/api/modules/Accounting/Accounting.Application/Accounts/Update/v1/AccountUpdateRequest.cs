using FSH.Framework.Core.Extensions.Dto;
using MediatR;

namespace Accounting.Application.Accounts.Update.v1;
public sealed record AccountUpdateRequest(
    string AccountCategory,
    string ParentCode,
    string Code,
    decimal Balance = 0)
    : BaseRequest, IRequest<AccountUpdateRequestResponse>;
