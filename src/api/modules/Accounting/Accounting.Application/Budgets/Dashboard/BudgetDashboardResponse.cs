using Accounting.Application.Dashboard;

namespace Accounting.Application.Budgets.Dashboard;

/// <summary>
/// Response containing comprehensive budget performance metrics and analytics.
/// </summary>
public sealed record BudgetDashboardResponse
{
    // Basic Budget Info
    public Guid BudgetId { get; init; }
    public string BudgetName { get; init; } = default!;
    public string BudgetType { get; init; } = default!;
    public string Status { get; init; } = default!;
    public int FiscalYear { get; init; }
    public Guid PeriodId { get; init; }
    public string PeriodName { get; init; } = default!;
    public string? Description { get; init; }
    public DateTime? ApprovedDate { get; init; }
    public string? ApprovedBy { get; init; }

    // Budget Overview
    public BudgetOverviewMetrics Overview { get; init; } = new();

    // Variance Analysis
    public BudgetVarianceMetrics Variance { get; init; } = new();

    // Detail Metrics
    public BudgetDetailMetrics Details { get; init; } = new();

    // Trend Data for Charts
    public List<AccountingTimeSeriesDataPoint> ActualTrend { get; init; } = [];
    public List<AccountingTimeSeriesDataPoint> UtilizationTrend { get; init; } = [];

    // Account Breakdown
    public List<BudgetAccountBreakdown> TopBudgetedAccounts { get; init; } = [];
    public List<BudgetAccountBreakdown> TopVarianceAccounts { get; init; } = [];

    // Monthly Performance (if applicable)
    public List<MonthlyComparisonData> MonthlyPerformance { get; init; } = [];

    // Alerts
    public List<DashboardAlert> Alerts { get; init; } = [];
}

public sealed record BudgetOverviewMetrics
{
    public decimal TotalBudgetedAmount { get; init; }
    public decimal TotalActualAmount { get; init; }
    public decimal RemainingBudget { get; init; }
    public decimal BudgetUtilization { get; init; }
    public bool IsOverBudget { get; init; }
    public int DetailLineCount { get; init; }
    public int AccountsWithActivity { get; init; }
    public int AccountsOverBudget { get; init; }
}

public sealed record BudgetVarianceMetrics
{
    public decimal TotalVariance { get; init; }
    public decimal TotalVariancePercentage { get; init; }
    public decimal FavorableVariance { get; init; }
    public decimal UnfavorableVariance { get; init; }
    public int FavorableLineCount { get; init; }
    public int UnfavorableLineCount { get; init; }
    public decimal LargestFavorableVariance { get; init; }
    public decimal LargestUnfavorableVariance { get; init; }
}

public sealed record BudgetDetailMetrics
{
    public int TotalDetails { get; init; }
    public int DetailsWithBudget { get; init; }
    public int DetailsWithActual { get; init; }
    public int DetailsWithNoActivity { get; init; }
    public decimal AverageBudgetPerLine { get; init; }
    public decimal AverageActualPerLine { get; init; }
    public decimal HighestBudgetedAmount { get; init; }
    public decimal HighestActualAmount { get; init; }
}

public sealed record BudgetAccountBreakdown
{
    public Guid AccountId { get; init; }
    public string AccountCode { get; init; } = default!;
    public string AccountName { get; init; } = default!;
    public decimal BudgetedAmount { get; init; }
    public decimal ActualAmount { get; init; }
    public decimal Variance { get; init; }
    public decimal VariancePercentage { get; init; }
    public decimal Utilization { get; init; }
    public bool IsOverBudget { get; init; }
}

public sealed record MonthlyComparisonData
{
    public string Month { get; init; } = default!;
    public int Year { get; init; }
    public decimal Amount { get; init; }
    public decimal PreviousAmount { get; init; }
    public decimal ChangePercentage { get; init; }
    public int TransactionCount { get; init; }
}

public sealed record DashboardAlert
{
    public string AlertType { get; init; } = default!;
    public string Severity { get; init; } = default!;
    public string Message { get; init; } = default!;
    public DateTime CreatedDate { get; init; }
    public Guid RelatedEntityId { get; init; }
    public string? RelatedEntityName { get; init; }
}
