using Accounting.Application.Dashboard;

namespace Accounting.Application.CostCenters.Dashboard;

/// <summary>
/// Query to retrieve comprehensive dashboard data for a specific cost center.
/// </summary>
public sealed record GetCostCenterDashboardQuery(Guid CostCenterId) : IRequest<CostCenterDashboardResponse>;

public sealed class GetCostCenterDashboardHandler(
    IReadRepository<CostCenter> costCenterRepository,
    IReadRepository<JournalEntryLine> journalEntryLineRepository,
    IReadRepository<JournalEntry> journalEntryRepository,
    IReadRepository<BillLineItem> billLineItemRepository,
    IReadRepository<Bill> billRepository,
    ICacheService cacheService,
    ILogger<GetCostCenterDashboardHandler> logger)
    : IRequestHandler<GetCostCenterDashboardQuery, CostCenterDashboardResponse>
{
    public async Task<CostCenterDashboardResponse> Handle(GetCostCenterDashboardQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"costcenter-dashboard:{request.CostCenterId}";

        var cachedResult = await cacheService.GetAsync<CostCenterDashboardResponse>(cacheKey, cancellationToken);
        if (cachedResult is not null)
        {
            return cachedResult;
        }

        logger.LogInformation("Generating dashboard for cost center {CostCenterId}", request.CostCenterId);

        var costCenter = await costCenterRepository.GetByIdAsync(request.CostCenterId, cancellationToken)
            ?? throw new NotFoundException($"Cost Center {request.CostCenterId} not found");

        var today = DateTime.UtcNow.Date;
        var startOfYear = new DateTime(today.Year, 1, 1);
        var startOfMonth = new DateTime(today.Year, today.Month, 1);
        var startOfLastMonth = startOfMonth.AddMonths(-1);

        // Get bill line items for this cost center
        var billLineItems = await billLineItemRepository.ListAsync(
            new BillLineItemsByCostCenterSpec(request.CostCenterId), cancellationToken);

        // Get related bills for dates
        var billIds = billLineItems.Select(l => l.BillId).Distinct().ToList();
        var bills = billIds.Count > 0
            ? await billRepository.ListAsync(new BillsByIdsSpec(billIds), cancellationToken)
            : new List<Bill>();
        var billDict = bills.ToDictionary(b => b.Id, b => b);

        // Get child cost centers
        var childCostCenters = await costCenterRepository.ListAsync(
            new ChildCostCentersSpec(request.CostCenterId), cancellationToken);

        // Calculate budget metrics
        var budget = CalculateBudgetMetrics(costCenter, billLineItems, billDict, startOfYear);

        // Calculate transaction metrics
        var transactions = CalculateTransactionMetrics(billLineItems, billDict, startOfYear, startOfMonth, startOfLastMonth, today);

        // Calculate child cost center summary
        var childSummary = CalculateChildCostCenterSummary(childCostCenters);

        // Generate trends
        var actualTrend = GenerateActualTrend(billLineItems, billDict, 12);
        var utilizationTrend = GenerateBudgetUtilizationTrend(costCenter, billLineItems, billDict, 12);

        // Expense breakdown by account
        var expenseBreakdown = CalculateExpenseBreakdown(billLineItems);

        // Recent transactions
        var recentTransactions = billLineItems
            .Select(l => new { LineItem = l, Bill = billDict.GetValueOrDefault(l.BillId) })
            .Where(x => x.Bill != null)
            .OrderByDescending(x => x.Bill!.BillDate)
            .Take(10)
            .Select(x => new RecentTransactionInfo
            {
                TransactionId = x.LineItem.Id,
                TransactionType = "Bill",
                ReferenceNumber = x.Bill!.BillNumber,
                TransactionDate = x.Bill.BillDate,
                Amount = x.LineItem.Amount,
                Description = x.LineItem.Description,
                Status = x.Bill.Status
            }).ToList();

        // Monthly performance
        var monthlyPerformance = GenerateMonthlyPerformance(billLineItems, billDict, 6);

        // Alerts
        var alerts = GenerateAlerts(costCenter, billLineItems, childCostCenters, today);

        var response = new CostCenterDashboardResponse
        {
            CostCenterId = costCenter.Id,
            Code = costCenter.Code,
            Name = costCenter.Name ?? "Unknown",
            CostCenterType = costCenter.CostCenterType.ToString(),
            IsActive = costCenter.IsActive,
            ManagerName = costCenter.ManagerName,
            Location = costCenter.Location,
            StartDate = costCenter.StartDate,
            EndDate = costCenter.EndDate,
            ParentCostCenterId = costCenter.ParentCostCenterId,
            Description = costCenter.Description,
            Budget = budget,
            Transactions = transactions,
            ChildCostCenters = childSummary,
            ActualTrend = actualTrend,
            BudgetUtilizationTrend = utilizationTrend,
            ExpenseBreakdown = expenseBreakdown,
            RecentTransactions = recentTransactions,
            MonthlyPerformance = monthlyPerformance,
            Alerts = alerts
        };

        await cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5), cancellationToken);

        return response;
    }

    private static CostCenterBudgetMetrics CalculateBudgetMetrics(
        CostCenter costCenter,
        List<BillLineItem> billLineItems,
        Dictionary<Guid, Bill> billDict,
        DateTime startOfYear)
    {
        var totalActual = billLineItems.Sum(l => l.Amount);
        var ytdLineItems = billLineItems
            .Where(l => billDict.TryGetValue(l.BillId, out var bill) && bill.BillDate >= startOfYear)
            .ToList();
        var ytdActual = ytdLineItems.Sum(l => l.Amount);

        var remaining = costCenter.BudgetAmount - costCenter.ActualAmount;
        var utilization = costCenter.BudgetAmount > 0
            ? costCenter.ActualAmount / costCenter.BudgetAmount * 100
            : 0;
        var variance = costCenter.BudgetAmount - costCenter.ActualAmount;
        var variancePercentage = costCenter.BudgetAmount > 0
            ? variance / costCenter.BudgetAmount * 100
            : 0;

        return new CostCenterBudgetMetrics
        {
            BudgetAmount = costCenter.BudgetAmount,
            ActualAmount = costCenter.ActualAmount,
            RemainingBudget = remaining,
            BudgetUtilization = utilization,
            Variance = variance,
            VariancePercentage = variancePercentage,
            IsOverBudget = costCenter.ActualAmount > costCenter.BudgetAmount,
            YTDActual = ytdActual,
            YTDVariance = costCenter.BudgetAmount - ytdActual
        };
    }

    private static CostCenterTransactionMetrics CalculateTransactionMetrics(
        List<BillLineItem> billLineItems,
        Dictionary<Guid, Bill> billDict,
        DateTime startOfYear,
        DateTime startOfMonth,
        DateTime startOfLastMonth,
        DateTime today)
    {
        var ytdItems = billLineItems
            .Where(l => billDict.TryGetValue(l.BillId, out var bill) && bill.BillDate >= startOfYear)
            .ToList();

        var thisMonthItems = billLineItems
            .Where(l => billDict.TryGetValue(l.BillId, out var bill) && bill.BillDate >= startOfMonth)
            .ToList();

        var lastMonthItems = billLineItems
            .Where(l => billDict.TryGetValue(l.BillId, out var bill) &&
                       bill.BillDate >= startOfLastMonth && bill.BillDate < startOfMonth)
            .ToList();

        var avgAmount = billLineItems.Count > 0 ? billLineItems.Average(l => l.Amount) : 0;
        var largestAmount = billLineItems.Count > 0 ? billLineItems.Max(l => l.Amount) : 0;

        DateTime? lastTransactionDate = null;
        var lastItem = billLineItems
            .Select(l => new { LineItem = l, Bill = billDict.GetValueOrDefault(l.BillId) })
            .Where(x => x.Bill != null)
            .OrderByDescending(x => x.Bill!.BillDate)
            .FirstOrDefault();

        if (lastItem?.Bill != null)
        {
            lastTransactionDate = lastItem.Bill.BillDate;
        }

        var daysSince = lastTransactionDate.HasValue
            ? (int)(today - lastTransactionDate.Value).TotalDays
            : 0;

        return new CostCenterTransactionMetrics
        {
            TotalTransactions = billLineItems.Count,
            TransactionsYTD = ytdItems.Count,
            TransactionsThisMonth = thisMonthItems.Count,
            TransactionsLastMonth = lastMonthItems.Count,
            AverageTransactionAmount = avgAmount,
            LargestTransaction = largestAmount,
            LastTransactionDate = lastTransactionDate,
            DaysSinceLastTransaction = daysSince
        };
    }

    private static ChildCostCenterSummary CalculateChildCostCenterSummary(List<CostCenter> childCostCenters)
    {
        var activeChildren = childCostCenters.Where(c => c.IsActive).ToList();
        var combinedBudget = childCostCenters.Sum(c => c.BudgetAmount);
        var combinedActual = childCostCenters.Sum(c => c.ActualAmount);

        var topChildren = childCostCenters
            .OrderByDescending(c => c.ActualAmount)
            .Take(5)
            .Select(c => new ChildCostCenterInfo
            {
                CostCenterId = c.Id,
                Code = c.Code,
                Name = c.Name ?? "Unknown",
                BudgetAmount = c.BudgetAmount,
                ActualAmount = c.ActualAmount,
                BudgetUtilization = c.BudgetAmount > 0 ? c.ActualAmount / c.BudgetAmount * 100 : 0
            }).ToList();

        return new ChildCostCenterSummary
        {
            TotalChildCostCenters = childCostCenters.Count,
            ActiveChildCostCenters = activeChildren.Count,
            CombinedBudget = combinedBudget,
            CombinedActual = combinedActual,
            TopChildren = topChildren
        };
    }

    private static List<TimeSeriesDataPoint> GenerateActualTrend(
        List<BillLineItem> billLineItems,
        Dictionary<Guid, Bill> billDict,
        int months)
    {
        var result = new List<TimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);

            var monthAmount = billLineItems
                .Where(l => billDict.TryGetValue(l.BillId, out var bill) &&
                           bill.BillDate >= monthStart && bill.BillDate < monthEnd)
                .Sum(l => l.Amount);

            result.Add(new TimeSeriesDataPoint
            {
                Date = monthStart,
                Value = monthAmount,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<TimeSeriesDataPoint> GenerateBudgetUtilizationTrend(
        CostCenter costCenter,
        List<BillLineItem> billLineItems,
        Dictionary<Guid, Bill> billDict,
        int months)
    {
        var result = new List<TimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;
        decimal cumulativeAmount = 0;

        var allMonths = new List<DateTime>();
        for (int i = months - 1; i >= 0; i--)
        {
            allMonths.Add(new DateTime(today.Year, today.Month, 1).AddMonths(-i));
        }

        foreach (var monthStart in allMonths)
        {
            var monthEnd = monthStart.AddMonths(1);
            var monthAmount = billLineItems
                .Where(l => billDict.TryGetValue(l.BillId, out var bill) &&
                           bill.BillDate >= monthStart && bill.BillDate < monthEnd)
                .Sum(l => l.Amount);

            cumulativeAmount += monthAmount;
            var utilization = costCenter.BudgetAmount > 0
                ? cumulativeAmount / costCenter.BudgetAmount * 100
                : 0;

            result.Add(new TimeSeriesDataPoint
            {
                Date = monthStart,
                Value = Math.Min(100, utilization),
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<AccountBreakdown> CalculateExpenseBreakdown(List<BillLineItem> billLineItems)
    {
        var totalAmount = billLineItems.Sum(l => l.Amount);

        return billLineItems
            .GroupBy(l => l.ChartOfAccountId)
            .Select(g => new AccountBreakdown
            {
                AccountId = g.Key,
                AccountCode = g.Key.ToString(),
                AccountName = "Account",
                Amount = g.Sum(l => l.Amount),
                Percentage = totalAmount > 0 ? g.Sum(l => l.Amount) / totalAmount * 100 : 0
            })
            .OrderByDescending(a => a.Amount)
            .Take(10)
            .ToList();
    }

    private static List<MonthlyComparisonData> GenerateMonthlyPerformance(
        List<BillLineItem> billLineItems,
        Dictionary<Guid, Bill> billDict,
        int months)
    {
        var result = new List<MonthlyComparisonData>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);
            var prevMonthStart = monthStart.AddMonths(-1);

            var monthAmount = billLineItems
                .Where(l => billDict.TryGetValue(l.BillId, out var bill) &&
                           bill.BillDate >= monthStart && bill.BillDate < monthEnd)
                .Sum(l => l.Amount);

            var prevAmount = billLineItems
                .Where(l => billDict.TryGetValue(l.BillId, out var bill) &&
                           bill.BillDate >= prevMonthStart && bill.BillDate < monthStart)
                .Sum(l => l.Amount);

            var change = prevAmount > 0 ? (monthAmount - prevAmount) / prevAmount * 100 : 0;

            result.Add(new MonthlyComparisonData
            {
                Month = monthStart.ToString("MMMM"),
                Year = monthStart.Year,
                Amount = monthAmount,
                PreviousAmount = prevAmount,
                ChangePercentage = change,
                TransactionCount = billLineItems.Count(l =>
                    billDict.TryGetValue(l.BillId, out var bill) &&
                    bill.BillDate >= monthStart && bill.BillDate < monthEnd)
            });
        }

        return result;
    }

    private static List<DashboardAlert> GenerateAlerts(
        CostCenter costCenter,
        List<BillLineItem> billLineItems,
        List<CostCenter> childCostCenters,
        DateTime today)
    {
        var alerts = new List<DashboardAlert>();

        // Over budget alert
        if (costCenter.ActualAmount > costCenter.BudgetAmount && costCenter.BudgetAmount > 0)
        {
            var overBy = costCenter.ActualAmount - costCenter.BudgetAmount;
            alerts.Add(new DashboardAlert
            {
                AlertType = "Over Budget",
                Severity = "Critical",
                Message = $"Cost center is over budget by {overBy:C}",
                CreatedDate = today,
                RelatedEntityId = costCenter.Id,
                RelatedEntityName = costCenter.Name
            });
        }

        // Near budget limit (>80%)
        var utilization = costCenter.BudgetAmount > 0
            ? costCenter.ActualAmount / costCenter.BudgetAmount * 100
            : 0;
        if (utilization >= 80 && utilization < 100)
        {
            alerts.Add(new DashboardAlert
            {
                AlertType = "Near Budget Limit",
                Severity = "Warning",
                Message = $"Cost center has used {utilization:F1}% of budget",
                CreatedDate = today,
                RelatedEntityId = costCenter.Id,
                RelatedEntityName = costCenter.Name
            });
        }

        // Inactive cost center alert
        if (!costCenter.IsActive)
        {
            alerts.Add(new DashboardAlert
            {
                AlertType = "Inactive",
                Severity = "Info",
                Message = $"Cost center {costCenter.Code} is inactive",
                CreatedDate = today,
                RelatedEntityId = costCenter.Id,
                RelatedEntityName = costCenter.Name
            });
        }

        // Child cost centers over budget
        var overBudgetChildren = childCostCenters.Where(c => c.ActualAmount > c.BudgetAmount && c.BudgetAmount > 0).ToList();
        if (overBudgetChildren.Count > 0)
        {
            alerts.Add(new DashboardAlert
            {
                AlertType = "Child Over Budget",
                Severity = "Warning",
                Message = $"{overBudgetChildren.Count} child cost center(s) over budget",
                CreatedDate = today,
                RelatedEntityId = costCenter.Id,
                RelatedEntityName = costCenter.Name
            });
        }

        return alerts.Take(5).ToList();
    }
}

// Specification classes
public sealed class BillLineItemsByCostCenterSpec : Specification<BillLineItem>
{
    public BillLineItemsByCostCenterSpec(Guid costCenterId)
    {
        Query.Where(l => l.CostCenterId == costCenterId);
    }
}

public sealed class BillsByIdsSpec : Specification<Bill>
{
    public BillsByIdsSpec(List<Guid> ids)
    {
        Query.Where(b => ids.Contains(b.Id));
    }
}

public sealed class ChildCostCentersSpec : Specification<CostCenter>
{
    public ChildCostCentersSpec(Guid parentCostCenterId)
    {
        Query.Where(c => c.ParentCostCenterId == parentCostCenterId);
    }
}
