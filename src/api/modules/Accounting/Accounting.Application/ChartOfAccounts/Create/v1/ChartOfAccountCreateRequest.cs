using FSH.Framework.Core.Extensions.Dto;
using MediatR;

namespace Accounting.Application.ChartOfAccounts.Create.v1;

public class ChartOfAccountCreateRequest
    : BaseRequest, IRequest<DefaultIdType>
{
    public string AccountCategory { get; set; } = null!;
    public string AccountType { get; set; } = null!;
    public string ParentCode { get; set; } = null!;
    public string AccountCode { get; set; } = null!;
    public decimal Balance { get; set; }
}
