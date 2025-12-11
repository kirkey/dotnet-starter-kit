using Accounting.Application.Dashboard;

namespace Accounting.Application.Projects.Dashboard;

/// <summary>
/// Query to retrieve comprehensive dashboard data for a specific project.
/// </summary>
public sealed record GetProjectDashboardQuery(Guid ProjectId) : IRequest<ProjectDashboardResponse>;

public sealed class GetProjectDashboardHandler(
    IReadRepository<Project> projectRepository,
    IReadRepository<ProjectCostEntry> costEntryRepository,
    ICacheService cacheService,
    ILogger<GetProjectDashboardHandler> logger)
    : IRequestHandler<GetProjectDashboardQuery, ProjectDashboardResponse>
{
    public async Task<ProjectDashboardResponse> Handle(GetProjectDashboardQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"project-dashboard:{request.ProjectId}";

        var cachedResult = await cacheService.GetAsync<ProjectDashboardResponse>(cacheKey, cancellationToken);
        if (cachedResult is not null)
        {
            return cachedResult;
        }

        logger.LogInformation("Generating dashboard for project {ProjectId}", request.ProjectId);

        var project = await projectRepository.GetByIdAsync(request.ProjectId, cancellationToken)
            ?? throw new NotFoundException($"Project {request.ProjectId} not found");

        var today = DateTime.UtcNow.Date;
        var startOfYear = new DateTime(today.Year, 1, 1);
        var startOfMonth = new DateTime(today.Year, today.Month, 1);
        var startOfLastMonth = startOfMonth.AddMonths(-1);

        // Get all cost entries for this project
        var costEntries = await costEntryRepository.ListAsync(
            new ProjectCostEntriesByProjectSpec(request.ProjectId), cancellationToken);

        // Calculate budget metrics
        var budget = CalculateBudgetMetrics(project, costEntries);

        // Calculate cost metrics
        var costs = CalculateCostMetrics(costEntries, startOfMonth, startOfLastMonth);

        // Calculate revenue metrics
        var revenue = CalculateRevenueMetrics(project, startOfMonth, startOfLastMonth, startOfYear);

        // Calculate timeline metrics
        var timeline = CalculateTimelineMetrics(project, costEntries, today);

        // Generate trends
        var costTrend = GenerateCostTrend(costEntries, 12);
        var revenueTrend = GenerateRevenueTrend(project, 12);
        var utilizationTrend = GenerateBudgetUtilizationTrend(project, costEntries, 12);

        // Cost category breakdown
        var costByCategory = CalculateCostCategoryBreakdown(costEntries);

        // Recent cost entries
        var recentCosts = costEntries
            .OrderByDescending(c => c.EntryDate)
            .Take(10)
            .Select(c => new RecentProjectCostInfo
            {
                CostEntryId = c.Id,
                EntryDate = c.EntryDate,
                Amount = c.Amount,
                Category = c.Category,
                Description = c.Description,
                CostCenter = c.CostCenter,
                IsBillable = c.IsBillable,
                IsApproved = c.IsApproved
            }).ToList();

        // Monthly performance
        var monthlyPerformance = GenerateMonthlyPerformance(costEntries, 6);

        // Alerts
        var alerts = GenerateAlerts(project, costEntries, today);

        var response = new ProjectDashboardResponse
        {
            ProjectId = project.Id,
            ProjectName = project.Name ?? "Unknown",
            Status = project.Status,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            ClientName = project.ClientName,
            ProjectManager = project.ProjectManager,
            Department = project.Department,
            Description = project.Description,
            Budget = budget,
            Costs = costs,
            Revenue = revenue,
            Timeline = timeline,
            CostTrend = costTrend,
            RevenueTrend = revenueTrend,
            BudgetUtilizationTrend = utilizationTrend,
            CostByCategory = costByCategory,
            RecentCosts = recentCosts,
            MonthlyPerformance = monthlyPerformance,
            Alerts = alerts
        };

        await cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5), cancellationToken);

        return response;
    }

    private static ProjectBudgetMetrics CalculateBudgetMetrics(Project project, List<ProjectCostEntry> costEntries)
    {
        var totalCost = costEntries.Sum(c => c.Amount);
        var remainingBudget = project.BudgetedAmount - totalCost;
        var utilization = project.BudgetedAmount > 0
            ? totalCost / project.BudgetedAmount * 100
            : 0;
        var variance = project.BudgetedAmount - totalCost;
        var variancePercentage = project.BudgetedAmount > 0
            ? variance / project.BudgetedAmount * 100
            : 0;

        var grossMargin = project.ActualRevenue - project.ActualCost;
        var grossMarginPercentage = project.ActualRevenue > 0
            ? grossMargin / project.ActualRevenue * 100
            : 0;

        return new ProjectBudgetMetrics
        {
            BudgetedAmount = project.BudgetedAmount,
            ActualCost = project.ActualCost,
            ActualRevenue = project.ActualRevenue,
            RemainingBudget = remainingBudget,
            BudgetUtilization = utilization,
            CostVariance = variance,
            CostVariancePercentage = variancePercentage,
            GrossMargin = grossMargin,
            GrossMarginPercentage = grossMarginPercentage,
            IsOverBudget = totalCost > project.BudgetedAmount
        };
    }

    private static ProjectCostMetrics CalculateCostMetrics(
        List<ProjectCostEntry> costEntries,
        DateTime startOfMonth,
        DateTime startOfLastMonth)
    {
        var thisMonthEntries = costEntries.Where(c => c.EntryDate >= startOfMonth).ToList();
        var lastMonthEntries = costEntries.Where(c => c.EntryDate >= startOfLastMonth && c.EntryDate < startOfMonth).ToList();
        var billableEntries = costEntries.Where(c => c.IsBillable).ToList();

        return new ProjectCostMetrics
        {
            TotalCostEntries = costEntries.Count,
            CostEntriesThisMonth = thisMonthEntries.Count,
            CostEntriesLastMonth = lastMonthEntries.Count,
            AverageCostEntry = costEntries.Count > 0 ? costEntries.Average(c => c.Amount) : 0,
            LargestCostEntry = costEntries.Count > 0 ? costEntries.Max(c => c.Amount) : 0,
            ApprovedEntries = costEntries.Count(c => c.IsApproved),
            PendingEntries = costEntries.Count(c => !c.IsApproved),
            BillableEntries = billableEntries.Count,
            BillableAmount = billableEntries.Sum(c => c.Amount)
        };
    }

    private static ProjectRevenueMetrics CalculateRevenueMetrics(
        Project project,
        DateTime startOfMonth,
        DateTime startOfLastMonth,
        DateTime startOfYear)
    {
        // Revenue is tracked at project level, estimate monthly based on duration
        var daysActive = (DateTime.UtcNow.Date - project.StartDate).Days;
        var monthsActive = Math.Max(1, daysActive / 30);
        var avgMonthlyRevenue = monthsActive > 0 ? project.ActualRevenue / monthsActive : 0;

        var roi = project.ActualCost > 0
            ? (project.ActualRevenue - project.ActualCost) / project.ActualCost * 100
            : 0;

        return new ProjectRevenueMetrics
        {
            TotalRevenue = project.ActualRevenue,
            RevenueThisMonth = avgMonthlyRevenue, // Estimated
            RevenueLastMonth = avgMonthlyRevenue, // Estimated
            RevenueYTD = project.ActualRevenue, // Simplified
            AverageMonthlyRevenue = avgMonthlyRevenue,
            ReturnOnInvestment = roi
        };
    }

    private static ProjectTimelineMetrics CalculateTimelineMetrics(
        Project project,
        List<ProjectCostEntry> costEntries,
        DateTime today)
    {
        var daysActive = (today - project.StartDate).Days;
        var percentComplete = project.BudgetedAmount > 0
            ? project.ActualCost / project.BudgetedAmount * 100
            : 0;

        // Calculate burn rate (cost per day)
        var burnRate = daysActive > 0 ? project.ActualCost / daysActive : 0;

        // Estimate days to completion based on burn rate
        var remainingBudget = project.BudgetedAmount - project.ActualCost;
        var estimatedDays = burnRate > 0 ? (int)(remainingBudget / burnRate) : 0;
        var estimatedCompletionDate = burnRate > 0 && remainingBudget > 0
            ? today.AddDays(estimatedDays)
            : (DateTime?)null;

        // Check if project has an end date and if we're on schedule
        var isOnSchedule = true;
        var daysRemaining = 0;
        if (project.EndDate.HasValue)
        {
            daysRemaining = (project.EndDate.Value - today).Days;
            isOnSchedule = estimatedDays <= daysRemaining || daysRemaining < 0;
        }

        return new ProjectTimelineMetrics
        {
            DaysActive = daysActive,
            DaysRemaining = daysRemaining,
            PercentComplete = Math.Min(100, percentComplete),
            BurnRate = burnRate,
            EstimatedDaysToCompletion = estimatedDays,
            EstimatedCompletionDate = estimatedCompletionDate,
            IsOnSchedule = isOnSchedule
        };
    }

    private static List<AccountingTimeSeriesDataPoint> GenerateCostTrend(List<ProjectCostEntry> costEntries, int months)
    {
        var result = new List<AccountingTimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);
            var monthCost = costEntries
                .Where(c => c.EntryDate >= monthStart && c.EntryDate < monthEnd)
                .Sum(c => c.Amount);

            result.Add(new AccountingTimeSeriesDataPoint
            {
                Date = monthStart,
                Value = monthCost,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<AccountingTimeSeriesDataPoint> GenerateRevenueTrend(Project project, int months)
    {
        var result = new List<AccountingTimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;
        var daysActive = (today - project.StartDate).Days;
        var avgMonthlyRevenue = daysActive > 30 ? project.ActualRevenue / (daysActive / 30m) : project.ActualRevenue;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);

            // Only show revenue for months after project started
            var value = monthStart >= project.StartDate ? avgMonthlyRevenue : 0;

            result.Add(new AccountingTimeSeriesDataPoint
            {
                Date = monthStart,
                Value = value,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<AccountingTimeSeriesDataPoint> GenerateBudgetUtilizationTrend(
        Project project,
        List<ProjectCostEntry> costEntries,
        int months)
    {
        var result = new List<AccountingTimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;
        decimal cumulativeCost = 0;

        var allMonths = new List<DateTime>();
        for (int i = months - 1; i >= 0; i--)
        {
            allMonths.Add(new DateTime(today.Year, today.Month, 1).AddMonths(-i));
        }

        foreach (var monthStart in allMonths)
        {
            var monthEnd = monthStart.AddMonths(1);
            var monthCost = costEntries
                .Where(c => c.EntryDate >= monthStart && c.EntryDate < monthEnd)
                .Sum(c => c.Amount);

            cumulativeCost += monthCost;
            var utilization = project.BudgetedAmount > 0
                ? cumulativeCost / project.BudgetedAmount * 100
                : 0;

            result.Add(new AccountingTimeSeriesDataPoint
            {
                Date = monthStart,
                Value = Math.Min(100, utilization),
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<CostCategoryBreakdown> CalculateCostCategoryBreakdown(List<ProjectCostEntry> costEntries)
    {
        var totalAmount = costEntries.Sum(c => c.Amount);

        return costEntries
            .GroupBy(c => c.Category ?? "Uncategorized")
            .Select(g => new CostCategoryBreakdown
            {
                Category = g.Key,
                EntryCount = g.Count(),
                TotalAmount = g.Sum(c => c.Amount),
                Percentage = totalAmount > 0 ? g.Sum(c => c.Amount) / totalAmount * 100 : 0,
                AverageAmount = g.Average(c => c.Amount)
            })
            .OrderByDescending(c => c.TotalAmount)
            .Take(10)
            .ToList();
    }

    private static List<MonthlyComparisonData> GenerateMonthlyPerformance(
        List<ProjectCostEntry> costEntries,
        int months)
    {
        var result = new List<MonthlyComparisonData>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);
            var prevMonthStart = monthStart.AddMonths(-1);

            var monthCost = costEntries
                .Where(c => c.EntryDate >= monthStart && c.EntryDate < monthEnd)
                .Sum(c => c.Amount);

            var prevCost = costEntries
                .Where(c => c.EntryDate >= prevMonthStart && c.EntryDate < monthStart)
                .Sum(c => c.Amount);

            var change = prevCost > 0 ? (monthCost - prevCost) / prevCost * 100 : 0;

            result.Add(new MonthlyComparisonData
            {
                Month = monthStart.ToString("MMMM"),
                Year = monthStart.Year,
                Amount = monthCost,
                PreviousAmount = prevCost,
                ChangePercentage = change,
                TransactionCount = costEntries.Count(c => c.EntryDate >= monthStart && c.EntryDate < monthEnd)
            });
        }

        return result;
    }

    private static List<DashboardAlert> GenerateAlerts(
        Project project,
        List<ProjectCostEntry> costEntries,
        DateTime today)
    {
        var alerts = new List<DashboardAlert>();

        // Over budget alert
        if (project.ActualCost > project.BudgetedAmount)
        {
            var overBy = project.ActualCost - project.BudgetedAmount;
            alerts.Add(new DashboardAlert
            {
                AlertType = "Over Budget",
                Severity = "Critical",
                Message = $"Project is over budget by {overBy:C}",
                CreatedDate = today,
                RelatedEntityId = project.Id,
                RelatedEntityName = project.Name
            });
        }

        // Near budget limit (>80%)
        var utilization = project.BudgetedAmount > 0
            ? project.ActualCost / project.BudgetedAmount * 100
            : 0;
        if (utilization >= 80 && utilization < 100)
        {
            alerts.Add(new DashboardAlert
            {
                AlertType = "Near Budget Limit",
                Severity = "Warning",
                Message = $"Project has used {utilization:F1}% of budget",
                CreatedDate = today,
                RelatedEntityId = project.Id,
                RelatedEntityName = project.Name
            });
        }

        // Pending approvals
        var pendingCount = costEntries.Count(c => !c.IsApproved);
        if (pendingCount > 0)
        {
            alerts.Add(new DashboardAlert
            {
                AlertType = "Pending Approvals",
                Severity = "Info",
                Message = $"{pendingCount} cost entries pending approval",
                CreatedDate = today,
                RelatedEntityId = project.Id,
                RelatedEntityName = project.Name
            });
        }

        // Project status alerts
        if (project.Status == "On Hold")
        {
            alerts.Add(new DashboardAlert
            {
                AlertType = "Project On Hold",
                Severity = "Warning",
                Message = $"Project is currently on hold",
                CreatedDate = today,
                RelatedEntityId = project.Id,
                RelatedEntityName = project.Name
            });
        }

        // No recent activity
        var recentEntries = costEntries.Where(c => c.EntryDate >= today.AddDays(-30)).ToList();
        if (project.Status == "Active" && costEntries.Count > 0 && recentEntries.Count == 0)
        {
            alerts.Add(new DashboardAlert
            {
                AlertType = "No Recent Activity",
                Severity = "Info",
                Message = "No cost entries in the last 30 days",
                CreatedDate = today,
                RelatedEntityId = project.Id,
                RelatedEntityName = project.Name
            });
        }

        return alerts.Take(5).ToList();
    }
}

// Specification class
public sealed class ProjectCostEntriesByProjectSpec : Specification<ProjectCostEntry>
{
    public ProjectCostEntriesByProjectSpec(Guid projectId)
    {
        Query.Where(c => c.ProjectId == projectId)
            .OrderByDescending(c => c.EntryDate);
    }
}
