namespace Accounting.Application.TrialBalance.Queries.GenerateTrialBalance.v1;

public class TrialBalanceDto
{
    public DateTime AsOfDate { get; set; }
    public string ReportTitle { get; set; } = "Trial Balance";
    public List<TrialBalanceLineDto> Lines { get; set; } = new();
    public decimal TotalDebits { get; set; }
    public decimal TotalCredits { get; set; }
    public bool IsBalanced => TotalDebits == TotalCredits;
}

public class TrialBalanceLineDto
{
    public DefaultIdType AccountId { get; set; }
    public string AccountCode { get; set; } = null!;
    public string AccountName { get; set; } = null!;
    public string AccountType { get; set; } = null!;
    public decimal DebitBalance { get; set; }
    public decimal CreditBalance { get; set; }
}
