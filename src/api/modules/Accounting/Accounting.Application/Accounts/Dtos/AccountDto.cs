using FSH.Framework.Core.Extensions.Dto;

namespace Accounting.Application.Accounts.Dtos;

public class AccountDto : BaseDto
{
    public string AccountCategory { get; set; } = null!;
    public string AccountType { get; set; } = null!;
    public string ParentCode { get; set; } = null!;
    public string Code { get; set; } = null!;
    public decimal Balance { get; set; }
}
