namespace Accounting.Application.ChartOfAccounts.Create.v1;

/// <summary>
/// Command to create a new Chart of Account entry.
/// </summary>
public class CreateChartOfAccountCommand : BaseRequest, IRequest<DefaultIdType>
{
    public string AccountCode { get; set; } = null!;
    public string AccountName { get; set; } = null!;
    public string AccountType { get; set; } = null!;
    public string UsoaCategory { get; set; } = null!;

    /// <summary>
    /// Optional parent account id linking this account into a hierarchy.
    /// Maps to domain property <c>ParentAccountId</c> (stored in DB column SubAccountOf).
    /// </summary>
    public DefaultIdType? ParentAccountId { get; set; }

    public string ParentCode { get; set; } = string.Empty;
    public decimal Balance { get; set; } = 0;
    public bool IsControlAccount { get; set; }
    public string NormalBalance { get; set; } = "Debit";
    public bool IsUsoaCompliant { get; set; } = true;
    public string? RegulatoryClassification { get; set; }
}
