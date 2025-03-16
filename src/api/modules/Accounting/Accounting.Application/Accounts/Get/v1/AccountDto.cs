using FSH.Framework.Core.Extensions.Dto;
using Shared.Enums.Account;

namespace Accounting.Application.Accounts.Get.v1;

public abstract record AccountDto : BaseDto
{
    public string AccountCategory { get; set; }
    public string ParentCode { get; private set; } = default!;
    public string Code { get; private set; } = default!;
    public decimal Balance { get; set; }
}
