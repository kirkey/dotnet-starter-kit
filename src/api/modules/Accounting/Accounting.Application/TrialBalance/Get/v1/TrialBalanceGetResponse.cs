namespace Accounting.Application.TrialBalance.Get.v1;

/// <summary>
/// Response containing trial balance details with line items.
/// </summary>
public sealed record TrialBalanceGetResponse
{
    public DefaultIdType Id { get; init; }
    public string TrialBalanceNumber { get; init; } = string.Empty;
    public DefaultIdType PeriodId { get; init; }
    public DateTime GeneratedDate { get; init; }
    public DateTime PeriodStartDate { get; init; }
    public DateTime PeriodEndDate { get; init; }
    public decimal TotalDebits { get; init; }
    public decimal TotalCredits { get; init; }
    public decimal TotalAssets { get; init; }
    public decimal TotalLiabilities { get; init; }
    public decimal TotalEquity { get; init; }
    public decimal TotalRevenue { get; init; }
    public decimal TotalExpenses { get; init; }
    public decimal NetIncome { get; init; }
    public bool IsBalanced { get; init; }
    public decimal OutOfBalanceAmount { get; init; }
    public bool AccountingEquationBalances { get; init; }
    public string Status { get; init; } = string.Empty;
    public bool IncludeZeroBalances { get; init; }
    public int AccountCount { get; init; }
    public DateTime? FinalizedDate { get; init; }
    public string? FinalizedBy { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public List<TrialBalanceLineItemDto> LineItems { get; init; } = new();
    public DateTime CreatedOn { get; init; }
}

public sealed record TrialBalanceLineItemDto
{
    public string AccountCode { get; init; } = string.Empty;
    public string AccountName { get; init; } = string.Empty;
    public string AccountType { get; init; } = string.Empty;
    public decimal DebitBalance { get; init; }
    public decimal CreditBalance { get; init; }
    public decimal NetBalance { get; init; }
}

