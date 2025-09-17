namespace Accounting.Application.ChartOfAccounts.Update.v1;

public class UpdateChartOfAccountRequest : BaseRequest, IRequest<DefaultIdType>
{
    public string AccountCode { get; set; } = default!;
    public string? AccountName { get; set; }
    public string? AccountType { get; set; }
    public string? UsoaCategory { get; set; }
    public DefaultIdType? SubAccountOf { get; set; }
    public string? ParentCode { get; set; }
    public bool IsControlAccount { get; set; }
    public decimal Balance { get; set; }
    public string? NormalBalance { get; set; }
    public bool IsUsoaCompliant { get; set; }
    public string? RegulatoryClassification { get; set; }
}
