namespace Accounting.Application.ChartOfAccounts.Update.v1;

public class UpdateChartOfAccountCommand : BaseRequest, IRequest<DefaultIdType>
{
    public string AccountCode { get; set; } = null!;
    public string? AccountName { get; set; }
    public string? AccountType { get; set; }
    public string? UsoaCategory { get; set; }

    /// <summary>
    /// Optional parent account id linking this account into a hierarchy.
    /// Maps to domain property <c>ParentAccountId</c> (stored in DB column SubAccountOf).
    /// </summary>
    public DefaultIdType? ParentAccountId { get; set; }

    public string? ParentCode { get; set; }
    public bool IsControlAccount { get; set; }
    public decimal Balance { get; set; }
    public string? NormalBalance { get; set; }
    public bool IsUsoaCompliant { get; set; }
    public string? RegulatoryClassification { get; set; }
}
