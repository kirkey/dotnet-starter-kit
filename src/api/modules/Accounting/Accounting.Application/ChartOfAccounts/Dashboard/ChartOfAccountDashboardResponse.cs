using Accounting.Application.Dashboard;

namespace Accounting.Application.ChartOfAccounts.Dashboard;

/// <summary>
/// Response containing comprehensive chart of account analytics and activity metrics.
/// </summary>
public sealed record ChartOfAccountDashboardResponse
{
    // Basic Account Info
    public Guid AccountId { get; init; }
    public string AccountCode { get; init; } = default!;
    public string AccountName { get; init; } = default!;
    public string AccountType { get; init; } = default!;
    public string UsoaCategory { get; init; } = default!;
    public string NormalBalance { get; init; } = default!;
    public bool IsActive { get; init; }
    public bool IsControlAccount { get; init; }
    public bool AllowDirectPosting { get; init; }
    public int AccountLevel { get; init; }
    public string? ParentCode { get; init; }
    public Guid? ParentAccountId { get; init; }

    // Balance Overview
    public AccountBalanceMetrics Balances { get; init; } = new();

    // Activity Metrics
    public AccountActivityMetrics Activity { get; init; } = new();

    // Sub-Account Summary (if control account)
    public SubAccountSummary SubAccounts { get; init; } = new();

    // Trend Data for Charts
    public List<AccountingTimeSeriesDataPoint> BalanceTrend { get; init; } = [];
    public List<AccountingTimeSeriesDataPoint> DebitTrend { get; init; } = [];
    public List<AccountingTimeSeriesDataPoint> CreditTrend { get; init; } = [];

    // Period Breakdown
    public List<PeriodBreakdown> PeriodActivity { get; init; } = [];

    // Recent Transactions
    public List<RecentJournalEntryInfo> RecentTransactions { get; init; } = [];

    // Monthly Performance
    public List<MonthlyComparisonData> MonthlyPerformance { get; init; } = [];

    // Alerts
    public List<DashboardAlert> Alerts { get; init; } = [];
}

public sealed record AccountBalanceMetrics
{
    public decimal CurrentBalance { get; init; }
    public decimal BeginningBalance { get; init; }
    public decimal YtdDebits { get; init; }
    public decimal YtdCredits { get; init; }
    public decimal YtdNetChange { get; init; }
    public decimal LastMonthBalance { get; init; }
    public decimal LastYearBalance { get; init; }
    public decimal BalanceChangePercentage { get; init; }
}

public sealed record AccountActivityMetrics
{
    public int TotalTransactions { get; init; }
    public int TransactionsYTD { get; init; }
    public int TransactionsThisMonth { get; init; }
    public int TransactionsLastMonth { get; init; }
    public decimal AverageTransactionAmount { get; init; }
    public decimal LargestDebit { get; init; }
    public decimal LargestCredit { get; init; }
    public DateTime? LastTransactionDate { get; init; }
    public int DaysSinceLastTransaction { get; init; }
}

public sealed record SubAccountSummary
{
    public int TotalSubAccounts { get; init; }
    public int ActiveSubAccounts { get; init; }
    public decimal CombinedBalance { get; init; }
    public List<SubAccountInfo> TopSubAccounts { get; init; } = [];
}

public sealed record SubAccountInfo
{
    public Guid AccountId { get; init; }
    public string AccountCode { get; init; } = default!;
    public string AccountName { get; init; } = default!;
    public decimal Balance { get; init; }
    public decimal PercentageOfParent { get; init; }
}

public sealed record RecentJournalEntryInfo
{
    public Guid JournalEntryId { get; init; }
    public Guid JournalEntryLineId { get; init; }
    public string ReferenceNumber { get; init; } = default!;
    public DateTime Date { get; init; }
    public decimal DebitAmount { get; init; }
    public decimal CreditAmount { get; init; }
    public string? Memo { get; init; }
    public string? Source { get; init; }
    public bool IsPosted { get; init; }
}
