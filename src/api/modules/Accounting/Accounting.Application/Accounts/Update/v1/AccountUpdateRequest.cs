using FSH.Framework.Core.Extensions.Dto;
using MediatR;

namespace Accounting.Application.Accounts.Update.v1;

public class AccountUpdateRequest : BaseRequest, IRequest<DefaultIdType>
{
    public string AccountCategory { get; set; } = null!;
    public string AccountType { get; set; } = null!;
    public string ParentCode { get; set; } = null!;
    public string Code { get; set; } = null!;
    public decimal Balance { get; set; }
}
    
