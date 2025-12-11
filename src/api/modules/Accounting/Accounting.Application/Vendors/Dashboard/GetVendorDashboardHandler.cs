using Accounting.Application.Dashboard;

namespace Accounting.Application.Vendors.Dashboard;

/// <summary>
/// Query to retrieve comprehensive dashboard data for a specific vendor.
/// </summary>
public sealed record GetVendorDashboardQuery(Guid VendorId) : IRequest<VendorDashboardResponse>;

public sealed class GetVendorDashboardHandler(
    IReadRepository<Vendor> vendorRepository,
    IReadRepository<Bill> billRepository,
    IReadRepository<Check> checkRepository,
    IReadRepository<BillLineItem> billLineItemRepository,
    ICacheService cacheService,
    ILogger<GetVendorDashboardHandler> logger)
    : IRequestHandler<GetVendorDashboardQuery, VendorDashboardResponse>
{
    public async Task<VendorDashboardResponse> Handle(GetVendorDashboardQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"vendor-dashboard:{request.VendorId}";

        var cachedResult = await cacheService.GetAsync<VendorDashboardResponse>(cacheKey, cancellationToken);
        if (cachedResult is not null)
        {
            return cachedResult;
        }

        logger.LogInformation("Generating dashboard for vendor {VendorId}", request.VendorId);

        var vendor = await vendorRepository.GetByIdAsync(request.VendorId, cancellationToken)
            ?? throw new NotFoundException($"Vendor {request.VendorId} not found");

        var today = DateTime.UtcNow.Date;
        var startOfYear = new DateTime(today.Year, 1, 1);
        var startOfMonth = new DateTime(today.Year, today.Month, 1);
        var lastYearStart = startOfYear.AddYears(-1);
        var lastYearEnd = startOfYear;

        // Get all bills for this vendor
        var bills = await billRepository.ListAsync(new BillsByVendorSpec(request.VendorId), cancellationToken);

        // Get all checks issued to this vendor
        var checks = await checkRepository.ListAsync(new ChecksByVendorSpec(request.VendorId), cancellationToken);

        // Get bill line items for expense analysis
        var billIds = bills.Select(b => b.Id).ToList();
        var billLineItems = billIds.Count > 0
            ? await billLineItemRepository.ListAsync(new BillLineItemsByBillsSpec(billIds), cancellationToken)
            : new List<BillLineItem>();

        // Calculate financial metrics
        var financials = CalculateFinancialMetrics(bills, checks, startOfYear, lastYearStart, lastYearEnd, today);

        // Calculate bill metrics
        var billMetrics = CalculateBillMetrics(bills, startOfYear, today);

        // Calculate payment metrics
        var paymentMetrics = CalculatePaymentMetrics(checks, startOfYear, startOfMonth);

        // Calculate account metrics
        var accountMetrics = CalculateAccountMetrics(vendor, billLineItems);

        // Generate trends
        var billValueTrend = GenerateBillValueTrend(bills, 12);
        var paymentTrend = GeneratePaymentTrend(checks, 12);
        var balanceTrend = GenerateBalanceTrend(bills, checks, 12);

        // Expense breakdown
        var expenseBreakdown = CalculateExpenseBreakdown(billLineItems);

        // Recent bills
        var recentBills = bills
            .OrderByDescending(b => b.BillDate)
            .Take(10)
            .Select(b => new RecentBillInfo
            {
                BillId = b.Id,
                BillNumber = b.BillNumber,
                BillDate = b.BillDate,
                DueDate = b.DueDate,
                TotalAmount = b.TotalAmount,
                Status = b.Status,
                IsPosted = b.IsPosted,
                IsPaid = b.IsPaid
            }).ToList();

        // Recent payments (checks)
        var recentPayments = checks
            .Where(c => c.Status == "Issued" || c.Status == "Cleared")
            .OrderByDescending(c => c.CreatedOn)
            .Take(10)
            .Select(c => new RecentPaymentInfo
            {
                PaymentId = c.Id,
                PaymentNumber = c.CheckNumber,
                PaymentDate = c.CreatedOn.DateTime,
                Amount = c.Amount ?? 0,
                PaymentMethod = "Check",
                Status = c.Status
            }).ToList();

        // Monthly performance
        var monthlyPerformance = GenerateMonthlyPerformance(bills, checks, 6);

        // Alerts
        var alerts = GenerateAlerts(bills, vendor, today);

        var response = new VendorDashboardResponse
        {
            VendorId = vendor.Id,
            VendorCode = vendor.VendorCode,
            VendorName = vendor.Name ?? "Unknown",
            ContactPerson = vendor.ContactPerson,
            Email = vendor.Email,
            PhoneNumber = vendor.PhoneNumber,
            Address = vendor.Address,
            IsActive = vendor.IsActive,
            Terms = vendor.Terms,
            Financials = financials,
            Bills = billMetrics,
            Payments = paymentMetrics,
            Accounts = accountMetrics,
            BillValueTrend = billValueTrend,
            PaymentTrend = paymentTrend,
            BalanceTrend = balanceTrend,
            ExpenseBreakdown = expenseBreakdown,
            RecentBills = recentBills,
            RecentPayments = recentPayments,
            MonthlyPerformance = monthlyPerformance,
            Alerts = alerts
        };

        await cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5), cancellationToken);

        return response;
    }

    private static VendorFinancialMetrics CalculateFinancialMetrics(
        List<Bill> bills,
        List<Check> checks,
        DateTime startOfYear,
        DateTime lastYearStart,
        DateTime lastYearEnd,
        DateTime today)
    {
        var totalBills = bills.Sum(b => b.TotalAmount);
        var ytdBills = bills.Where(b => b.BillDate >= startOfYear).Sum(b => b.TotalAmount);
        var lastYearBills = bills.Where(b => b.BillDate >= lastYearStart && b.BillDate < lastYearEnd).Sum(b => b.TotalAmount);

        var issuedChecks = checks.Where(c => c.Status == "Issued" || c.Status == "Cleared").ToList();
        var totalPayments = issuedChecks.Sum(c => c.Amount ?? 0);
        var ytdPayments = issuedChecks.Where(c => c.CreatedOn >= startOfYear).Sum(c => c.Amount ?? 0);

        var unpaidBills = bills.Where(b => !b.IsPaid).Sum(b => b.TotalAmount);
        var overdueBills = bills.Where(b => !b.IsPaid && b.DueDate < today).Sum(b => b.TotalAmount);

        var avgBillAmount = bills.Count > 0 ? totalBills / bills.Count : 0;

        var growth = lastYearBills > 0
            ? (ytdBills - lastYearBills) / lastYearBills * 100
            : 0;

        return new VendorFinancialMetrics
        {
            TotalBillsAmount = totalBills,
            TotalBillsAmountYTD = ytdBills,
            TotalBillsAmountLastYear = lastYearBills,
            TotalPaymentsAmount = totalPayments,
            TotalPaymentsAmountYTD = ytdPayments,
            OutstandingBalance = unpaidBills,
            OverdueBalance = overdueBills,
            AverageBillAmount = avgBillAmount,
            GrowthPercentage = growth
        };
    }

    private static VendorBillMetrics CalculateBillMetrics(
        List<Bill> bills,
        DateTime startOfYear,
        DateTime today)
    {
        var ytdBills = bills.Where(b => b.BillDate >= startOfYear).ToList();

        var draftBills = bills.Count(b => b.Status == "Draft");
        var pendingBills = bills.Count(b => b.Status == "Pending");
        var approvedBills = bills.Count(b => b.Status == "Approved");
        var postedBills = bills.Count(b => b.IsPosted);
        var paidBills = bills.Count(b => b.IsPaid);
        var overdueBills = bills.Count(b => !b.IsPaid && b.DueDate < today);

        // Calculate average payment days for paid bills
        var paidBillsWithDates = bills.Where(b => b.IsPaid && b.PaidDate.HasValue).ToList();
        var avgPaymentDays = paidBillsWithDates.Count > 0
            ? paidBillsWithDates.Average(b => (b.PaidDate!.Value - b.BillDate).TotalDays)
            : 0;

        // Calculate on-time payment rate
        var onTimePayments = paidBillsWithDates.Count(b => b.PaidDate <= b.DueDate);
        var onTimeRate = paidBillsWithDates.Count > 0
            ? (decimal)onTimePayments / paidBillsWithDates.Count * 100
            : 0;

        return new VendorBillMetrics
        {
            TotalBills = bills.Count,
            TotalBillsYTD = ytdBills.Count,
            DraftBills = draftBills,
            PendingBills = pendingBills,
            ApprovedBills = approvedBills,
            PostedBills = postedBills,
            PaidBills = paidBills,
            OverdueBills = overdueBills,
            AveragePaymentDays = (decimal)avgPaymentDays,
            OnTimePaymentRate = onTimeRate
        };
    }

    private static VendorPaymentMetrics CalculatePaymentMetrics(
        List<Check> checks,
        DateTime startOfYear,
        DateTime startOfMonth)
    {
        var issuedChecks = checks.Where(c => c.Status == "Issued" || c.Status == "Cleared").ToList();
        var ytdChecks = issuedChecks.Where(c => c.CreatedOn >= startOfYear).ToList();
        var monthChecks = issuedChecks.Where(c => c.CreatedOn >= startOfMonth).ToList();

        return new VendorPaymentMetrics
        {
            TotalPayments = issuedChecks.Count,
            TotalPaymentsYTD = ytdChecks.Count,
            CheckPayments = issuedChecks.Count, // All are checks in this context
            AchPayments = 0,
            WirePayments = 0,
            AveragePaymentAmount = issuedChecks.Count > 0 ? issuedChecks.Average(c => c.Amount ?? 0) : 0,
            PaymentsThisMonth = monthChecks.Count,
            PaymentsThisMonthAmount = monthChecks.Sum(c => c.Amount ?? 0)
        };
    }

    private static VendorAccountMetrics CalculateAccountMetrics(
        Vendor vendor,
        List<BillLineItem> billLineItems)
    {
        var uniqueAccounts = billLineItems
            .Select(li => li.ChartOfAccountId)
            .Distinct()
            .Count();

        // Total amount across all line items (can't filter by default account without ChartOfAccount lookup)
        var totalAmount = billLineItems.Sum(li => li.Amount);

        return new VendorAccountMetrics
        {
            DefaultExpenseAccount = vendor.ExpenseAccountCode,
            DefaultExpenseAccountName = vendor.ExpenseAccountName,
            UniqueExpenseAccounts = uniqueAccounts,
            TotalByDefaultAccount = totalAmount
        };
    }

    private static List<TimeSeriesDataPoint> GenerateBillValueTrend(List<Bill> bills, int months)
    {
        var result = new List<TimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);
            var monthValue = bills
                .Where(b => b.BillDate >= monthStart && b.BillDate < monthEnd)
                .Sum(b => b.TotalAmount);

            result.Add(new TimeSeriesDataPoint
            {
                Date = monthStart,
                Value = monthValue,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<TimeSeriesDataPoint> GeneratePaymentTrend(List<Check> checks, int months)
    {
        var result = new List<TimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;
        var issuedChecks = checks.Where(c => c.Status == "Issued" || c.Status == "Cleared").ToList();

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);
            var monthValue = issuedChecks
                .Where(c => c.CreatedOn >= monthStart && c.CreatedOn < monthEnd)
                .Sum(c => c.Amount ?? 0);

            result.Add(new TimeSeriesDataPoint
            {
                Date = monthStart,
                Value = monthValue,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<TimeSeriesDataPoint> GenerateBalanceTrend(List<Bill> bills, List<Check> checks, int months)
    {
        var result = new List<TimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;
        var issuedChecks = checks.Where(c => c.Status == "Issued" || c.Status == "Cleared").ToList();

        decimal runningBalance = 0;
        var allMonths = new List<DateTime>();

        for (int i = months - 1; i >= 0; i--)
        {
            allMonths.Add(new DateTime(today.Year, today.Month, 1).AddMonths(-i));
        }

        foreach (var monthStart in allMonths)
        {
            var monthEnd = monthStart.AddMonths(1);

            var monthBills = bills.Where(b => b.BillDate >= monthStart && b.BillDate < monthEnd).Sum(b => b.TotalAmount);
            var monthPayments = issuedChecks.Where(c => c.CreatedOn >= monthStart && c.CreatedOn < monthEnd).Sum(c => c.Amount ?? 0);

            runningBalance += monthBills - monthPayments;

            result.Add(new TimeSeriesDataPoint
            {
                Date = monthStart,
                Value = runningBalance,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<AccountBreakdown> CalculateExpenseBreakdown(List<BillLineItem> billLineItems)
    {
        var totalAmount = billLineItems.Sum(li => li.Amount);

        return billLineItems
            .GroupBy(li => li.ChartOfAccountId)
            .Select(g => new AccountBreakdown
            {
                AccountId = g.Key,
                AccountCode = g.Key.ToString(),
                AccountName = "Account",
                Amount = g.Sum(li => li.Amount),
                Percentage = totalAmount > 0 ? g.Sum(li => li.Amount) / totalAmount * 100 : 0
            })
            .OrderByDescending(a => a.Amount)
            .Take(10)
            .ToList();
    }

    private static List<MonthlyComparisonData> GenerateMonthlyPerformance(
        List<Bill> bills,
        List<Check> checks,
        int months)
    {
        var result = new List<MonthlyComparisonData>();
        var today = DateTime.UtcNow.Date;
        var issuedChecks = checks.Where(c => c.Status == "Issued" || c.Status == "Cleared").ToList();

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);
            var prevMonthStart = monthStart.AddMonths(-1);
            var prevMonthEnd = monthStart;

            var monthBills = bills.Where(b => b.BillDate >= monthStart && b.BillDate < monthEnd).Sum(b => b.TotalAmount);
            var prevMonthBills = bills.Where(b => b.BillDate >= prevMonthStart && b.BillDate < prevMonthEnd).Sum(b => b.TotalAmount);

            var changePercentage = prevMonthBills > 0
                ? (monthBills - prevMonthBills) / prevMonthBills * 100
                : 0;

            result.Add(new MonthlyComparisonData
            {
                Month = monthStart.ToString("MMMM"),
                Year = monthStart.Year,
                Amount = monthBills,
                PreviousAmount = prevMonthBills,
                ChangePercentage = changePercentage,
                TransactionCount = bills.Count(b => b.BillDate >= monthStart && b.BillDate < monthEnd)
            });
        }

        return result;
    }

    private static List<DashboardAlert> GenerateAlerts(List<Bill> bills, Vendor vendor, DateTime today)
    {
        var alerts = new List<DashboardAlert>();

        // Overdue bills alert
        var overdueBills = bills.Where(b => !b.IsPaid && b.DueDate < today).ToList();
        if (overdueBills.Count > 0)
        {
            var totalOverdue = overdueBills.Sum(b => b.TotalAmount);
            alerts.Add(new DashboardAlert
            {
                AlertType = "Overdue Bills",
                Severity = "Warning",
                Message = $"{overdueBills.Count} overdue bills totaling {totalOverdue:C}",
                CreatedDate = today,
                RelatedEntityId = vendor.Id,
                RelatedEntityName = vendor.Name
            });
        }

        // Bills due soon alert
        var dueSoon = bills.Where(b => !b.IsPaid && b.DueDate >= today && b.DueDate <= today.AddDays(7)).ToList();
        if (dueSoon.Count > 0)
        {
            var totalDueSoon = dueSoon.Sum(b => b.TotalAmount);
            alerts.Add(new DashboardAlert
            {
                AlertType = "Bills Due Soon",
                Severity = "Info",
                Message = $"{dueSoon.Count} bills due within 7 days totaling {totalDueSoon:C}",
                CreatedDate = today,
                RelatedEntityId = vendor.Id,
                RelatedEntityName = vendor.Name
            });
        }

        // Inactive vendor alert
        if (!vendor.IsActive)
        {
            alerts.Add(new DashboardAlert
            {
                AlertType = "Inactive Vendor",
                Severity = "Warning",
                Message = $"Vendor {vendor.Name} is currently inactive",
                CreatedDate = today,
                RelatedEntityId = vendor.Id,
                RelatedEntityName = vendor.Name
            });
        }

        return alerts.Take(5).ToList();
    }
}

// Specification classes for vendor dashboard
public sealed class BillsByVendorSpec : Specification<Bill>
{
    public BillsByVendorSpec(Guid vendorId)
    {
        Query.Where(b => b.VendorId == vendorId)
            .OrderByDescending(b => b.BillDate);
    }
}

public sealed class ChecksByVendorSpec : Specification<Check>
{
    public ChecksByVendorSpec(Guid vendorId)
    {
        Query.Where(c => c.VendorId == vendorId)
            .OrderByDescending(c => c.CreatedOn);
    }
}

public sealed class BillLineItemsByBillsSpec : Specification<BillLineItem>
{
    public BillLineItemsByBillsSpec(List<Guid> billIds)
    {
        Query.Where(li => billIds.Contains(li.BillId));
    }
}
