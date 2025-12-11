using Accounting.Application.Dashboard;

namespace Accounting.Application.Budgets.Dashboard;

/// <summary>
/// Query to retrieve comprehensive dashboard data for a specific budget.
/// </summary>
public sealed record GetBudgetDashboardQuery(Guid BudgetId) : IRequest<BudgetDashboardResponse>;

public sealed class GetBudgetDashboardHandler(
    IReadRepository<Budget> budgetRepository,
    IReadRepository<BudgetDetail> budgetDetailRepository,
    IReadRepository<ChartOfAccount> chartOfAccountRepository,
    ICacheService cacheService,
    ILogger<GetBudgetDashboardHandler> logger)
    : IRequestHandler<GetBudgetDashboardQuery, BudgetDashboardResponse>
{
    public async Task<BudgetDashboardResponse> Handle(GetBudgetDashboardQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"budget-dashboard:{request.BudgetId}";

        var cachedResult = await cacheService.GetAsync<BudgetDashboardResponse>(cacheKey, cancellationToken);
        if (cachedResult is not null)
        {
            return cachedResult;
        }

        logger.LogInformation("Generating dashboard for budget {BudgetId}", request.BudgetId);

        var budget = await budgetRepository.GetByIdAsync(request.BudgetId, cancellationToken)
            ?? throw new NotFoundException($"Budget {request.BudgetId} not found");

        var today = DateTime.UtcNow.Date;

        // Get all budget details
        var budgetDetails = await budgetDetailRepository.ListAsync(
            new BudgetDetailsByBudgetSpec(request.BudgetId), cancellationToken);

        // Get accounts for lookup
        var accountIds = budgetDetails.Select(d => d.AccountId).Distinct().ToList();
        var accounts = accountIds.Count > 0
            ? await chartOfAccountRepository.ListAsync(new ChartOfAccountsByIdsSpec(accountIds), cancellationToken)
            : new List<ChartOfAccount>();
        var accountDict = accounts.ToDictionary(a => a.Id, a => a);

        // Calculate overview metrics
        var overview = CalculateOverviewMetrics(budget, budgetDetails);

        // Calculate variance metrics
        var variance = CalculateVarianceMetrics(budgetDetails);

        // Calculate detail metrics
        var details = CalculateDetailMetrics(budgetDetails);

        // Generate trends (simulated based on budget period)
        var actualTrend = GenerateActualTrend(budget, budgetDetails, 12);
        var utilizationTrend = GenerateUtilizationTrend(budget, budgetDetails, 12);

        // Account breakdowns
        var topBudgeted = CalculateTopBudgetedAccounts(budgetDetails, accountDict);
        var topVariance = CalculateTopVarianceAccounts(budgetDetails, accountDict);

        // Monthly performance (simulated)
        var monthlyPerformance = GenerateMonthlyPerformance(budget, 6);

        // Alerts
        var alerts = GenerateAlerts(budget, budgetDetails, today);

        var response = new BudgetDashboardResponse
        {
            BudgetId = budget.Id,
            BudgetName = budget.Name ?? "Unknown",
            BudgetType = budget.BudgetType,
            Status = budget.Status,
            FiscalYear = budget.FiscalYear,
            PeriodId = budget.PeriodId,
            PeriodName = budget.PeriodName,
            Description = budget.Description,
            ApprovedDate = budget.ApprovedOn,
            ApprovedBy = budget.ApproverName,
            Overview = overview,
            Variance = variance,
            Details = details,
            ActualTrend = actualTrend,
            UtilizationTrend = utilizationTrend,
            TopBudgetedAccounts = topBudgeted,
            TopVarianceAccounts = topVariance,
            MonthlyPerformance = monthlyPerformance,
            Alerts = alerts
        };

        await cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5), cancellationToken);

        return response;
    }

    private static BudgetOverviewMetrics CalculateOverviewMetrics(Budget budget, List<BudgetDetail> details)
    {
        var accountsWithActivity = details.Count(d => d.ActualAmount > 0);
        var accountsOverBudget = details.Count(d => d.ActualAmount > d.BudgetedAmount);

        return new BudgetOverviewMetrics
        {
            TotalBudgetedAmount = budget.TotalBudgetedAmount,
            TotalActualAmount = budget.TotalActualAmount,
            RemainingBudget = budget.TotalBudgetedAmount - budget.TotalActualAmount,
            BudgetUtilization = budget.TotalBudgetedAmount > 0
                ? budget.TotalActualAmount / budget.TotalBudgetedAmount * 100
                : 0,
            IsOverBudget = budget.TotalActualAmount > budget.TotalBudgetedAmount,
            DetailLineCount = details.Count,
            AccountsWithActivity = accountsWithActivity,
            AccountsOverBudget = accountsOverBudget
        };
    }

    private static BudgetVarianceMetrics CalculateVarianceMetrics(List<BudgetDetail> details)
    {
        var totalVariance = details.Sum(d => d.BudgetedAmount - d.ActualAmount);
        var totalBudgeted = details.Sum(d => d.BudgetedAmount);
        var variancePercentage = totalBudgeted > 0
            ? totalVariance / totalBudgeted * 100
            : 0;

        var favorableDetails = details.Where(d => d.ActualAmount < d.BudgetedAmount).ToList();
        var unfavorableDetails = details.Where(d => d.ActualAmount > d.BudgetedAmount).ToList();

        var favorableVariance = favorableDetails.Sum(d => d.BudgetedAmount - d.ActualAmount);
        var unfavorableVariance = unfavorableDetails.Sum(d => d.ActualAmount - d.BudgetedAmount);

        var largestFavorable = favorableDetails.Count > 0
            ? favorableDetails.Max(d => d.BudgetedAmount - d.ActualAmount)
            : 0;
        var largestUnfavorable = unfavorableDetails.Count > 0
            ? unfavorableDetails.Max(d => d.ActualAmount - d.BudgetedAmount)
            : 0;

        return new BudgetVarianceMetrics
        {
            TotalVariance = totalVariance,
            TotalVariancePercentage = variancePercentage,
            FavorableVariance = favorableVariance,
            UnfavorableVariance = unfavorableVariance,
            FavorableLineCount = favorableDetails.Count,
            UnfavorableLineCount = unfavorableDetails.Count,
            LargestFavorableVariance = largestFavorable,
            LargestUnfavorableVariance = largestUnfavorable
        };
    }

    private static BudgetDetailMetrics CalculateDetailMetrics(List<BudgetDetail> details)
    {
        var detailsWithBudget = details.Count(d => d.BudgetedAmount > 0);
        var detailsWithActual = details.Count(d => d.ActualAmount > 0);
        var detailsNoActivity = details.Count(d => d.BudgetedAmount > 0 && d.ActualAmount == 0);

        var avgBudget = details.Count > 0 ? details.Average(d => d.BudgetedAmount) : 0;
        var avgActual = details.Count > 0 ? details.Average(d => d.ActualAmount) : 0;

        var highestBudget = details.Count > 0 ? details.Max(d => d.BudgetedAmount) : 0;
        var highestActual = details.Count > 0 ? details.Max(d => d.ActualAmount) : 0;

        return new BudgetDetailMetrics
        {
            TotalDetails = details.Count,
            DetailsWithBudget = detailsWithBudget,
            DetailsWithActual = detailsWithActual,
            DetailsWithNoActivity = detailsNoActivity,
            AverageBudgetPerLine = avgBudget,
            AverageActualPerLine = avgActual,
            HighestBudgetedAmount = highestBudget,
            HighestActualAmount = highestActual
        };
    }

    private static List<AccountingTimeSeriesDataPoint> GenerateActualTrend(
        Budget budget,
        List<BudgetDetail> details,
        int months)
    {
        var result = new List<AccountingTimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;
        var totalActual = details.Sum(d => d.ActualAmount);

        // Simulate monthly progression based on total actuals
        var monthlyAvg = months > 0 ? totalActual / months : 0;
        decimal cumulative = 0;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);

            // Simulate progressive spending
            var monthValue = i == 0 ? totalActual - cumulative : monthlyAvg;
            cumulative += monthValue;

            result.Add(new AccountingTimeSeriesDataPoint
            {
                Date = monthStart,
                Value = cumulative,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<AccountingTimeSeriesDataPoint> GenerateUtilizationTrend(
        Budget budget,
        List<BudgetDetail> details,
        int months)
    {
        var result = new List<AccountingTimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;
        var totalActual = details.Sum(d => d.ActualAmount);
        var totalBudget = budget.TotalBudgetedAmount;

        var monthlyAvg = months > 0 ? totalActual / months : 0;
        decimal cumulative = 0;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthValue = i == 0 ? totalActual - cumulative : monthlyAvg;
            cumulative += monthValue;

            var utilization = totalBudget > 0 ? cumulative / totalBudget * 100 : 0;

            result.Add(new AccountingTimeSeriesDataPoint
            {
                Date = monthStart,
                Value = Math.Min(100, utilization),
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<BudgetAccountBreakdown> CalculateTopBudgetedAccounts(
        List<BudgetDetail> details,
        Dictionary<Guid, ChartOfAccount> accountDict)
    {
        return details
            .OrderByDescending(d => d.BudgetedAmount)
            .Take(10)
            .Select(d =>
            {
                var account = accountDict.GetValueOrDefault(d.AccountId);
                var variance = d.BudgetedAmount - d.ActualAmount;
                var variancePercentage = d.BudgetedAmount > 0
                    ? variance / d.BudgetedAmount * 100
                    : 0;
                var utilization = d.BudgetedAmount > 0
                    ? d.ActualAmount / d.BudgetedAmount * 100
                    : 0;

                return new BudgetAccountBreakdown
                {
                    AccountId = d.AccountId,
                    AccountCode = account?.AccountCode ?? d.AccountId.ToString(),
                    AccountName = account?.AccountName ?? "Unknown",
                    BudgetedAmount = d.BudgetedAmount,
                    ActualAmount = d.ActualAmount,
                    Variance = variance,
                    VariancePercentage = variancePercentage,
                    Utilization = utilization,
                    IsOverBudget = d.ActualAmount > d.BudgetedAmount
                };
            }).ToList();
    }

    private static List<BudgetAccountBreakdown> CalculateTopVarianceAccounts(
        List<BudgetDetail> details,
        Dictionary<Guid, ChartOfAccount> accountDict)
    {
        return details
            .OrderByDescending(d => Math.Abs(d.BudgetedAmount - d.ActualAmount))
            .Take(10)
            .Select(d =>
            {
                var account = accountDict.GetValueOrDefault(d.AccountId);
                var variance = d.BudgetedAmount - d.ActualAmount;
                var variancePercentage = d.BudgetedAmount > 0
                    ? variance / d.BudgetedAmount * 100
                    : 0;
                var utilization = d.BudgetedAmount > 0
                    ? d.ActualAmount / d.BudgetedAmount * 100
                    : 0;

                return new BudgetAccountBreakdown
                {
                    AccountId = d.AccountId,
                    AccountCode = account?.AccountCode ?? d.AccountId.ToString(),
                    AccountName = account?.AccountName ?? "Unknown",
                    BudgetedAmount = d.BudgetedAmount,
                    ActualAmount = d.ActualAmount,
                    Variance = variance,
                    VariancePercentage = variancePercentage,
                    Utilization = utilization,
                    IsOverBudget = d.ActualAmount > d.BudgetedAmount
                };
            }).ToList();
    }

    private static List<MonthlyComparisonData> GenerateMonthlyPerformance(Budget budget, int months)
    {
        var result = new List<MonthlyComparisonData>();
        var today = DateTime.UtcNow.Date;
        var monthlyBudget = budget.TotalBudgetedAmount / 12; // Simplified monthly allocation

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);

            result.Add(new MonthlyComparisonData
            {
                Month = monthStart.ToString("MMMM"),
                Year = monthStart.Year,
                Amount = monthlyBudget,
                PreviousAmount = monthlyBudget,
                ChangePercentage = 0,
                TransactionCount = 0
            });
        }

        return result;
    }

    private static List<DashboardAlert> GenerateAlerts(
        Budget budget,
        List<BudgetDetail> details,
        DateTime today)
    {
        var alerts = new List<DashboardAlert>();

        // Over budget alert
        if (budget.TotalActualAmount > budget.TotalBudgetedAmount)
        {
            var overBy = budget.TotalActualAmount - budget.TotalBudgetedAmount;
            alerts.Add(new DashboardAlert
            {
                AlertType = "Over Budget",
                Severity = "Critical",
                Message = $"Budget exceeded by {overBy:C}",
                CreatedDate = today,
                RelatedEntityId = budget.Id,
                RelatedEntityName = budget.Name
            });
        }

        // Near budget limit (>80%)
        var utilization = budget.TotalBudgetedAmount > 0
            ? budget.TotalActualAmount / budget.TotalBudgetedAmount * 100
            : 0;
        if (utilization >= 80 && utilization < 100)
        {
            alerts.Add(new DashboardAlert
            {
                AlertType = "Near Budget Limit",
                Severity = "Warning",
                Message = $"Budget is {utilization:F1}% utilized",
                CreatedDate = today,
                RelatedEntityId = budget.Id,
                RelatedEntityName = budget.Name
            });
        }

        // Status alerts
        if (budget.Status == "Draft")
        {
            alerts.Add(new DashboardAlert
            {
                AlertType = "Draft Status",
                Severity = "Info",
                Message = "Budget is still in draft status",
                CreatedDate = today,
                RelatedEntityId = budget.Id,
                RelatedEntityName = budget.Name
            });
        }

        // Accounts over budget
        var overBudgetAccounts = details.Count(d => d.ActualAmount > d.BudgetedAmount);
        if (overBudgetAccounts > 0)
        {
            alerts.Add(new DashboardAlert
            {
                AlertType = "Accounts Over Budget",
                Severity = "Warning",
                Message = $"{overBudgetAccounts} account(s) are over budget",
                CreatedDate = today,
                RelatedEntityId = budget.Id,
                RelatedEntityName = budget.Name
            });
        }

        // Accounts with no activity
        var noActivityAccounts = details.Count(d => d.BudgetedAmount > 0 && d.ActualAmount == 0);
        if (noActivityAccounts > 5)
        {
            alerts.Add(new DashboardAlert
            {
                AlertType = "Low Activity",
                Severity = "Info",
                Message = $"{noActivityAccounts} budgeted accounts have no activity",
                CreatedDate = today,
                RelatedEntityId = budget.Id,
                RelatedEntityName = budget.Name
            });
        }

        return alerts.Take(5).ToList();
    }
}

// Specification classes
public sealed class BudgetDetailsByBudgetSpec : Specification<BudgetDetail>
{
    public BudgetDetailsByBudgetSpec(Guid budgetId)
    {
        Query.Where(d => d.BudgetId == budgetId);
    }
}

public sealed class ChartOfAccountsByIdsSpec : Specification<ChartOfAccount>
{
    public ChartOfAccountsByIdsSpec(List<Guid> ids)
    {
        Query.Where(a => ids.Contains(a.Id));
    }
}
