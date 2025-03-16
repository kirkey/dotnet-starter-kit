using System.ComponentModel;
using FSH.Framework.Core.Extensions.Dto;
using MediatR;

namespace Accounting.Application.Accounts.Create.v1;
public sealed record AccountCreateRequest(
    [property: DefaultValue("")] string AccountCategory,
    [property: DefaultValue("")] string ParentCode,
    [property: DefaultValue("")] string Code,
    [property: DefaultValue("0.00")] decimal Balance)
    : BaseRequest, IRequest<AccountCreateRequestResponse>;
