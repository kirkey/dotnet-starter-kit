using FSH.Framework.Core.Extensions.Dto;

namespace Accounting.Application.ChartOfAccounts.Dtos;

public class ChartOfAccountDto : BaseDto
{
    public string AccountCategory { get; set; } = null!;
    public string AccountType { get; set; } = null!;
    public string ParentCode { get; set; } = null!;
    public string AccountCode { get; set; } = null!;
    public decimal Balance { get; set; }
}
