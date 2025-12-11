using Accounting.Application.Dashboard;

namespace Accounting.Application.CostCenters.Dashboard;

/// <summary>
/// Response containing comprehensive cost center performance metrics and analytics.
/// </summary>
public sealed record CostCenterDashboardResponse
{
    // Basic Cost Center Info
    public Guid CostCenterId { get; init; }
    public string Code { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string CostCenterType { get; init; } = default!;
    public bool IsActive { get; init; }
    public string? ManagerName { get; init; }
    public string? Location { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public Guid? ParentCostCenterId { get; init; }
    public string? Description { get; init; }

    // Budget vs Actual
    public CostCenterBudgetMetrics Budget { get; init; } = new();

    // Transaction Metrics
    public CostCenterTransactionMetrics Transactions { get; init; } = new();

    // Child Cost Centers (if parent)
    public ChildCostCenterSummary ChildCostCenters { get; init; } = new();

    // Trend Data for Charts
    public List<AccountingTimeSeriesDataPoint> ActualTrend { get; init; } = [];
    public List<AccountingTimeSeriesDataPoint> BudgetUtilizationTrend { get; init; } = [];

    // Account Breakdown
    public List<AccountBreakdown> ExpenseBreakdown { get; init; } = [];

    // Recent Transactions
    public List<RecentTransactionInfo> RecentTransactions { get; init; } = [];

    // Monthly Performance
    public List<MonthlyComparisonData> MonthlyPerformance { get; init; } = [];

    // Alerts
    public List<DashboardAlert> Alerts { get; init; } = [];
}

public sealed record CostCenterBudgetMetrics
{
    public decimal BudgetAmount { get; init; }
    public decimal ActualAmount { get; init; }
    public decimal RemainingBudget { get; init; }
    public decimal BudgetUtilization { get; init; }
    public decimal Variance { get; init; }
    public decimal VariancePercentage { get; init; }
    public bool IsOverBudget { get; init; }
    public decimal YTDActual { get; init; }
    public decimal YTDVariance { get; init; }
}

public sealed record CostCenterTransactionMetrics
{
    public int TotalTransactions { get; init; }
    public int TransactionsYTD { get; init; }
    public int TransactionsThisMonth { get; init; }
    public int TransactionsLastMonth { get; init; }
    public decimal AverageTransactionAmount { get; init; }
    public decimal LargestTransaction { get; init; }
    public DateTime? LastTransactionDate { get; init; }
    public int DaysSinceLastTransaction { get; init; }
}

public sealed record ChildCostCenterSummary
{
    public int TotalChildCostCenters { get; init; }
    public int ActiveChildCostCenters { get; init; }
    public decimal CombinedBudget { get; init; }
    public decimal CombinedActual { get; init; }
    public List<ChildCostCenterInfo> TopChildren { get; init; } = [];
}

public sealed record ChildCostCenterInfo
{
    public Guid CostCenterId { get; init; }
    public string Code { get; init; } = default!;
    public string Name { get; init; } = default!;
    public decimal BudgetAmount { get; init; }
    public decimal ActualAmount { get; init; }
    public decimal BudgetUtilization { get; init; }
}
