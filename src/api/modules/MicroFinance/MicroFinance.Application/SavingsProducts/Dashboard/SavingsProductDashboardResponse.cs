namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Dashboard;

/// <summary>
/// Comprehensive dashboard analytics for a savings product
/// </summary>
public sealed record SavingsProductDashboardResponse(
    SavingsProductOverview Overview,
    AccountStatistics AccountStats,
    BalanceMetrics BalanceMetrics,
    TransactionMetrics TransactionMetrics,
    InterestMetrics InterestMetrics,
    List<AccountStatusDistribution> StatusDistribution,
    List<MonthlyActivityTrend> MonthlyTrends,
    List<RecentAccountActivity> RecentAccounts,
    SavingsProductAlerts Alerts,
    DateTime GeneratedAt);

/// <summary>
/// Basic product overview information
/// </summary>
public sealed record SavingsProductOverview(
    Guid ProductId,
    string Code,
    string Name,
    string? Description,
    decimal InterestRate,
    string InterestCalculation,
    string InterestPostingFrequency,
    decimal MinOpeningBalance,
    decimal MinBalanceForInterest,
    decimal MinWithdrawalAmount,
    decimal? MaxWithdrawalPerDay,
    bool AllowOverdraft,
    decimal? OverdraftLimit,
    bool IsActive);

/// <summary>
/// Account statistics by status
/// </summary>
public sealed record AccountStatistics(
    int TotalAccounts,
    int ActiveAccounts,
    int PendingAccounts,
    int DormantAccounts,
    int ClosedAccounts,
    int FrozenAccounts,
    int NewAccountsLast30Days,
    int NewAccountsLast12Months,
    decimal AccountClosureRate,
    decimal DormancyRate);

/// <summary>
/// Aggregate balance metrics
/// </summary>
public sealed record BalanceMetrics(
    decimal TotalBalance,
    decimal TotalDeposits,
    decimal TotalWithdrawals,
    decimal TotalInterestEarned,
    decimal AverageBalance,
    decimal MedianBalance,
    decimal MinBalance,
    decimal MaxBalance,
    int AccountsBelowMinimum,
    decimal PercentageBelowMinimum);

/// <summary>
/// Transaction volume and trends
/// </summary>
public sealed record TransactionMetrics(
    int TotalTransactionsLast30Days,
    decimal TotalDepositAmountLast30Days,
    decimal TotalWithdrawalAmountLast30Days,
    int DepositCount,
    int WithdrawalCount,
    int InterestPostingCount,
    int FeeTransactionCount,
    int TransferInCount,
    int TransferOutCount,
    decimal AverageDepositAmount,
    decimal AverageWithdrawalAmount);

/// <summary>
/// Interest-related metrics
/// </summary>
public sealed record InterestMetrics(
    decimal TotalInterestPaidThisMonth,
    decimal TotalInterestPaidThisYear,
    decimal AverageInterestPerAccount,
    int AccountsDueForPosting,
    DateOnly? LastInterestPostingDate,
    decimal ProjectedMonthlyInterest);

/// <summary>
/// Distribution of accounts by status
/// </summary>
public sealed record AccountStatusDistribution(
    string Status,
    int Count,
    decimal TotalBalance,
    decimal Percentage);

/// <summary>
/// Monthly activity trends
/// </summary>
public sealed record MonthlyActivityTrend(
    string Month,
    int Year,
    int NewAccounts,
    int ClosedAccounts,
    decimal TotalDeposits,
    decimal TotalWithdrawals,
    decimal NetFlow,
    decimal InterestPaid,
    int TransactionCount);

/// <summary>
/// Recent account activity details
/// </summary>
public sealed record RecentAccountActivity(
    Guid AccountId,
    string AccountNumber,
    string MemberName,
    decimal CurrentBalance,
    string Status,
    DateOnly OpenedDate,
    DateOnly? LastTransactionDate,
    decimal TotalInterestEarned);

/// <summary>
/// Alerts and notifications for the product
/// </summary>
public sealed record SavingsProductAlerts(
    int DormantAccountsCount,
    int AccountsBelowMinimumCount,
    int FrozenAccountsCount,
    int PendingActivationCount,
    List<SavingsProductAlert> AlertList);

/// <summary>
/// Individual alert item
/// </summary>
public sealed record SavingsProductAlert(
    string AlertType,
    string Severity,
    string Message,
    int AffectedCount,
    DateTime CreatedAt);
