namespace Accounting.Application.AccountsReceivableAccounts.Queries;

/// <summary>
/// Accounts receivable account data transfer object for list views.
/// </summary>
public record AccountsReceivableAccountDto
{
    public DefaultIdType Id { get; init; }
    public string AccountNumber { get; init; } = string.Empty;
    public string AccountName { get; init; } = string.Empty;
    public decimal CurrentBalance { get; init; }
    public decimal NetReceivables { get; init; }
    public decimal AllowanceForDoubtfulAccounts { get; init; }
    public bool IsReconciled { get; init; }
    public DateTime? LastReconciledDate { get; init; }
}

/// <summary>
/// Accounts receivable account data transfer object for detail view with all properties.
/// </summary>
public record AccountsReceivableAccountDetailsDto : AccountsReceivableAccountDto
{
    public decimal Current0to30 { get; init; }
    public decimal Days31to60 { get; init; }
    public decimal Days61to90 { get; init; }
    public decimal Over90Days { get; init; }
    public decimal YearToDateCollections { get; init; }
    public decimal YearToDateWriteOffs { get; init; }
    public decimal BadDebtPercentage { get; init; }
    public decimal DaysSalesOutstanding { get; init; }
    public int CustomerCount { get; init; }
    public decimal VarianceAmount { get; init; }
    public DefaultIdType? GeneralLedgerAccountId { get; init; }
    public DefaultIdType? PeriodId { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
}

