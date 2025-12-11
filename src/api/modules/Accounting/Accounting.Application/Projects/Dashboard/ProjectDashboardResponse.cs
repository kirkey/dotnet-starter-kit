using Accounting.Application.Dashboard;

namespace Accounting.Application.Projects.Dashboard;

/// <summary>
/// Response containing comprehensive project performance metrics and analytics.
/// </summary>
public sealed record ProjectDashboardResponse
{
    // Basic Project Info
    public Guid ProjectId { get; init; }
    public string ProjectName { get; init; } = default!;
    public string Status { get; init; } = default!;
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string? ClientName { get; init; }
    public string? ProjectManager { get; init; }
    public string? Department { get; init; }
    public string? Description { get; init; }

    // Budget vs Actual
    public ProjectBudgetMetrics Budget { get; init; } = new();

    // Cost Breakdown
    public ProjectCostMetrics Costs { get; init; } = new();

    // Revenue Metrics
    public ProjectRevenueMetrics Revenue { get; init; } = new();

    // Timeline Metrics
    public ProjectTimelineMetrics Timeline { get; init; } = new();

    // Trend Data for Charts
    public List<AccountingTimeSeriesDataPoint> CostTrend { get; init; } = [];
    public List<AccountingTimeSeriesDataPoint> RevenueTrend { get; init; } = [];
    public List<AccountingTimeSeriesDataPoint> BudgetUtilizationTrend { get; init; } = [];

    // Cost Category Breakdown
    public List<ProjectCostCategoryBreakdown> CostByCategory { get; init; } = [];

    // Recent Cost Entries
    public List<RecentProjectCostInfo> RecentCosts { get; init; } = [];

    // Monthly Performance
    public List<MonthlyComparisonData> MonthlyPerformance { get; init; } = [];

    // Alerts
    public List<DashboardAlert> Alerts { get; init; } = [];
}

public sealed record ProjectBudgetMetrics
{
    public decimal BudgetedAmount { get; init; }
    public decimal ActualCost { get; init; }
    public decimal ActualRevenue { get; init; }
    public decimal RemainingBudget { get; init; }
    public decimal BudgetUtilization { get; init; }
    public decimal CostVariance { get; init; }
    public decimal CostVariancePercentage { get; init; }
    public decimal GrossMargin { get; init; }
    public decimal GrossMarginPercentage { get; init; }
    public bool IsOverBudget { get; init; }
}

public sealed record ProjectCostMetrics
{
    public int TotalCostEntries { get; init; }
    public int CostEntriesThisMonth { get; init; }
    public int CostEntriesLastMonth { get; init; }
    public decimal AverageCostEntry { get; init; }
    public decimal LargestCostEntry { get; init; }
    public int ApprovedEntries { get; init; }
    public int PendingEntries { get; init; }
    public int BillableEntries { get; init; }
    public decimal BillableAmount { get; init; }
}

public sealed record ProjectRevenueMetrics
{
    public decimal TotalRevenue { get; init; }
    public decimal RevenueThisMonth { get; init; }
    public decimal RevenueLastMonth { get; init; }
    public decimal RevenueYTD { get; init; }
    public decimal AverageMonthlyRevenue { get; init; }
    public decimal ReturnOnInvestment { get; init; }
}

public sealed record ProjectTimelineMetrics
{
    public int DaysActive { get; init; }
    public int DaysRemaining { get; init; }
    public decimal PercentComplete { get; init; }
    public decimal BurnRate { get; init; }
    public int EstimatedDaysToCompletion { get; init; }
    public DateTime? EstimatedCompletionDate { get; init; }
    public bool IsOnSchedule { get; init; }
}

public sealed record ProjectCostCategoryBreakdown
{
    public string Category { get; init; } = default!;
    public int EntryCount { get; init; }
    public decimal TotalAmount { get; init; }
    public decimal Percentage { get; init; }
    public decimal AverageAmount { get; init; }
}

public sealed record RecentProjectCostInfo
{
    public Guid CostEntryId { get; init; }
    public DateTime EntryDate { get; init; }
    public decimal Amount { get; init; }
    public string? Category { get; init; }
    public string? Description { get; init; }
    public string? CostCenter { get; init; }
    public bool IsBillable { get; init; }
    public bool IsApproved { get; init; }
}
