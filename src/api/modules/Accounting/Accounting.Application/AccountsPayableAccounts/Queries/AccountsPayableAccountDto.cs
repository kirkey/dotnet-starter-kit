namespace Accounting.Application.AccountsPayableAccounts.Queries;

/// <summary>
/// Accounts payable account data transfer object for list views.
/// </summary>
public record AccountsPayableAccountDto
{
    public DefaultIdType Id { get; init; }
    public string AccountNumber { get; init; } = string.Empty;
    public string AccountName { get; init; } = string.Empty;
    public decimal CurrentBalance { get; init; }
    public bool IsReconciled { get; init; }
    public DateTime? LastReconciledDate { get; init; }
}

/// <summary>
/// Accounts payable account data transfer object for detail view with all properties.
/// </summary>
public record AccountsPayableAccountDetailsDto : AccountsPayableAccountDto
{
    public decimal Current0to30 { get; init; }
    public decimal Days31to60 { get; init; }
    public decimal Days61to90 { get; init; }
    public decimal Over90Days { get; init; }
    public decimal YearToDatePayments { get; init; }
    public decimal DiscountsTaken { get; init; }
    public decimal DiscountsLost { get; init; }
    public decimal DaysPayableOutstanding { get; init; }
    public int VendorCount { get; init; }
    public decimal VarianceAmount { get; init; }
    public DefaultIdType? GeneralLedgerAccountId { get; init; }
    public DefaultIdType? PeriodId { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
}

