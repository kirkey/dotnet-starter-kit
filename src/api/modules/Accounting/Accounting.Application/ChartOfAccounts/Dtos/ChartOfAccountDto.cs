namespace Accounting.Application.ChartOfAccounts.Dtos;

public class ChartOfAccountResponse : BaseDto
{
    public string AccountCode { get; set; } = null!;
    public string AccountType { get; set; } = null!;
    public DefaultIdType? SubAccountOf { get; set; } = null!;
    public string UsoaCategory { get; set; } = null!;
    public bool IsActive { get; set; }
    public string ParentCode { get; set; } = null!;
    public decimal Balance { get; set; }
    public bool IsControlAccount { get; set; }
    public string NormalBalance { get; set; } = null!;
    public int AccountLevel { get; set; }
}
