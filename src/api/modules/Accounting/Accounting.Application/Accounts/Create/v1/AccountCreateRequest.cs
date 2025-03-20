using FSH.Framework.Core.Extensions.Dto;
using MediatR;

namespace Accounting.Application.Accounts.Create.v1;

public class AccountCreateRequest
    : BaseRequest, IRequest<DefaultIdType>
{
    public string AccountCategory { get; set; } = null!;
    public string AccountType { get; set; } = null!;
    public string ParentCode { get; set; } = null!;
    public string Code { get; set; } = null!;
    public decimal Balance { get; set; }
}
