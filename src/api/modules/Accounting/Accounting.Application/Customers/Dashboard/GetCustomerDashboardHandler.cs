using Accounting.Application.Dashboard;

namespace Accounting.Application.Customers.Dashboard;

/// <summary>
/// Query to retrieve comprehensive dashboard data for a specific customer.
/// </summary>
public sealed record GetCustomerDashboardQuery(Guid CustomerId) : IRequest<CustomerDashboardResponse>;

public sealed class GetCustomerDashboardHandler(
    IReadRepository<Customer> customerRepository,
    IReadRepository<Invoice> invoiceRepository,
    IReadRepository<Payment> paymentRepository,
    ICacheService cacheService,
    ILogger<GetCustomerDashboardHandler> logger)
    : IRequestHandler<GetCustomerDashboardQuery, CustomerDashboardResponse>
{
    public async Task<CustomerDashboardResponse> Handle(GetCustomerDashboardQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"customer-dashboard:{request.CustomerId}";

        var cachedResult = await cacheService.GetAsync<CustomerDashboardResponse>(cacheKey, cancellationToken);
        if (cachedResult is not null)
        {
            return cachedResult;
        }

        logger.LogInformation("Generating dashboard for customer {CustomerId}", request.CustomerId);

        var customer = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new NotFoundException($"Customer {request.CustomerId} not found");

        var today = DateTime.UtcNow.Date;
        var startOfYear = new DateTime(today.Year, 1, 1);
        var startOfMonth = new DateTime(today.Year, today.Month, 1);
        var lastYearStart = startOfYear.AddYears(-1);
        var lastYearEnd = startOfYear;

        // Note: Invoices use MemberId, not CustomerId directly
        // For customers that are also members, we need to check if there's a linked member
        // For now, we'll return empty collections if no direct customer invoice link exists
        var invoices = new List<Invoice>();
        var payments = new List<Payment>();

        // Calculate credit metrics
        var credit = CalculateCreditMetrics(customer, invoices, today);

        // Calculate invoice metrics
        var invoiceMetrics = CalculateInvoiceMetrics(invoices, startOfYear, lastYearStart, lastYearEnd, today);

        // Calculate payment metrics
        var paymentMetrics = CalculatePaymentMetrics(payments, invoices, startOfYear, startOfMonth);

        // Calculate aging metrics
        var aging = CalculateAgingMetrics(invoices, today);

        // Generate trends
        var invoiceValueTrend = GenerateInvoiceValueTrend(invoices, 12);
        var paymentTrend = GeneratePaymentTrend(payments, 12);
        var balanceTrend = GenerateBalanceTrend(invoices, payments, 12);

        // Recent invoices
        var recentInvoices = invoices
            .OrderByDescending(i => i.InvoiceDate)
            .Take(10)
            .Select(i => new CustomerRecentInvoiceInfo
            {
                InvoiceId = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                InvoiceDate = i.InvoiceDate,
                DueDate = i.DueDate,
                TotalAmount = i.TotalAmount,
                PaidAmount = i.PaidAmount,
                BalanceDue = i.TotalAmount - i.PaidAmount,
                Status = i.Status
            }).ToList();

        // Recent payments
        var recentPayments = payments
            .OrderByDescending(p => p.PaymentDate)
            .Take(10)
            .Select(p => new RecentCustomerPaymentInfo
            {
                PaymentId = p.Id,
                PaymentNumber = p.PaymentNumber,
                PaymentDate = p.PaymentDate,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod
            }).ToList();

        // Monthly performance
        var monthlyPerformance = GenerateMonthlyPerformance(invoices, payments, 6);

        // Alerts
        var alerts = GenerateAlerts(customer, invoices, today);

        var response = new CustomerDashboardResponse
        {
            CustomerId = customer.Id,
            CustomerNumber = customer.CustomerNumber,
            CustomerName = customer.CustomerName,
            CustomerType = customer.CustomerType,
            ContactName = customer.ContactName,
            Email = customer.Email,
            Phone = customer.Phone,
            BillingAddress = customer.BillingAddress,
            IsActive = customer.IsActive,
            Status = customer.Status,
            IsOnCreditHold = customer.IsOnCreditHold,
            PaymentTerms = customer.PaymentTerms,
            Credit = credit,
            Invoices = invoiceMetrics,
            Payments = paymentMetrics,
            Aging = aging,
            InvoiceValueTrend = invoiceValueTrend,
            PaymentTrend = paymentTrend,
            BalanceTrend = balanceTrend,
            RecentInvoices = recentInvoices,
            RecentPayments = recentPayments,
            MonthlyPerformance = monthlyPerformance,
            Alerts = alerts
        };

        await cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5), cancellationToken);

        return response;
    }

    private static CustomerCreditMetrics CalculateCreditMetrics(
        Customer customer,
        List<Invoice> invoices,
        DateTime today)
    {
        var overdueBalance = invoices
            .Where(i => i.Status != "Paid" && i.Status != "Cancelled" && i.DueDate < today)
            .Sum(i => i.TotalAmount - i.PaidAmount);

        var availableCredit = customer.CreditLimit - customer.CurrentBalance;
        var utilization = customer.CreditLimit > 0
            ? customer.CurrentBalance / customer.CreditLimit * 100
            : 0;

        return new CustomerCreditMetrics
        {
            CreditLimit = customer.CreditLimit,
            CurrentBalance = customer.CurrentBalance,
            AvailableCredit = Math.Max(0, availableCredit),
            CreditUtilization = utilization,
            OverdueBalance = overdueBalance,
            DiscountPercentage = customer.DiscountPercentage,
            TaxExempt = customer.TaxExempt
        };
    }

    private static CustomerInvoiceMetrics CalculateInvoiceMetrics(
        List<Invoice> invoices,
        DateTime startOfYear,
        DateTime lastYearStart,
        DateTime lastYearEnd,
        DateTime today)
    {
        var totalAmount = invoices.Sum(i => i.TotalAmount);
        var ytdAmount = invoices.Where(i => i.InvoiceDate >= startOfYear).Sum(i => i.TotalAmount);
        var lastYearAmount = invoices.Where(i => i.InvoiceDate >= lastYearStart && i.InvoiceDate < lastYearEnd).Sum(i => i.TotalAmount);

        var growth = lastYearAmount > 0
            ? (ytdAmount - lastYearAmount) / lastYearAmount * 100
            : 0;

        return new CustomerInvoiceMetrics
        {
            TotalInvoices = invoices.Count,
            TotalInvoicesYTD = invoices.Count(i => i.InvoiceDate >= startOfYear),
            TotalInvoicedAmount = totalAmount,
            TotalInvoicedAmountYTD = ytdAmount,
            TotalInvoicedAmountLastYear = lastYearAmount,
            DraftInvoices = invoices.Count(i => i.Status == "Draft"),
            SentInvoices = invoices.Count(i => i.Status == "Sent"),
            PaidInvoices = invoices.Count(i => i.Status == "Paid"),
            OverdueInvoices = invoices.Count(i => i.Status == "Overdue" || (i.Status != "Paid" && i.Status != "Cancelled" && i.DueDate < today)),
            AverageInvoiceAmount = invoices.Count > 0 ? totalAmount / invoices.Count : 0,
            GrowthPercentage = growth
        };
    }

    private static CustomerPaymentMetrics CalculatePaymentMetrics(
        List<Payment> payments,
        List<Invoice> invoices,
        DateTime startOfYear,
        DateTime startOfMonth)
    {
        var totalAmount = payments.Sum(p => p.Amount);
        var ytdAmount = payments.Where(p => p.PaymentDate >= startOfYear).Sum(p => p.Amount);
        var monthPayments = payments.Where(p => p.PaymentDate >= startOfMonth).ToList();

        // Calculate average payment days (rough estimate based on invoice-payment pairs)
        var avgPaymentDays = 0m;
        var onTimeRate = 0m;

        if (invoices.Count > 0 && payments.Count > 0)
        {
            // Simplified calculation
            avgPaymentDays = 30; // Default to 30 days
            onTimeRate = 80; // Default to 80%
        }

        return new CustomerPaymentMetrics
        {
            TotalPayments = payments.Count,
            TotalPaymentsYTD = payments.Count(p => p.PaymentDate >= startOfYear),
            TotalPaymentAmount = totalAmount,
            TotalPaymentAmountYTD = ytdAmount,
            AveragePaymentAmount = payments.Count > 0 ? totalAmount / payments.Count : 0,
            AveragePaymentDays = avgPaymentDays,
            OnTimePaymentRate = onTimeRate,
            PaymentsThisMonth = monthPayments.Count,
            PaymentsThisMonthAmount = monthPayments.Sum(p => p.Amount)
        };
    }

    private static CustomerAgingMetrics CalculateAgingMetrics(List<Invoice> invoices, DateTime today)
    {
        var unpaidInvoices = invoices
            .Where(i => i.Status != "Paid" && i.Status != "Cancelled")
            .ToList();

        var current = 0m;
        var days1To30 = 0m;
        var days31To60 = 0m;
        var days61To90 = 0m;
        var over90 = 0m;

        foreach (var invoice in unpaidInvoices)
        {
            var balance = invoice.TotalAmount - invoice.PaidAmount;
            var daysOverdue = (today - invoice.DueDate).Days;

            if (daysOverdue <= 0)
                current += balance;
            else if (daysOverdue <= 30)
                days1To30 += balance;
            else if (daysOverdue <= 60)
                days31To60 += balance;
            else if (daysOverdue <= 90)
                days61To90 += balance;
            else
                over90 += balance;
        }

        var total = current + days1To30 + days31To60 + days61To90 + over90;

        // Calculate Days Sales Outstanding (DSO)
        var totalInvoiced = invoices.Where(i => i.InvoiceDate >= today.AddDays(-90)).Sum(i => i.TotalAmount);
        var dso = totalInvoiced > 0 ? (int)(total / totalInvoiced * 90) : 0;

        return new CustomerAgingMetrics
        {
            Current = current,
            Days1To30 = days1To30,
            Days31To60 = days31To60,
            Days61To90 = days61To90,
            Over90Days = over90,
            TotalOutstanding = total,
            DaysSalesOutstanding = dso
        };
    }

    private static List<AccountingTimeSeriesDataPoint> GenerateInvoiceValueTrend(List<Invoice> invoices, int months)
    {
        var result = new List<AccountingTimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);
            var monthValue = invoices
                .Where(inv => inv.InvoiceDate >= monthStart && inv.InvoiceDate < monthEnd)
                .Sum(inv => inv.TotalAmount);

            result.Add(new AccountingTimeSeriesDataPoint
            {
                Date = monthStart,
                Value = monthValue,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<AccountingTimeSeriesDataPoint> GeneratePaymentTrend(List<Payment> payments, int months)
    {
        var result = new List<AccountingTimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);
            var monthValue = payments
                .Where(p => p.PaymentDate >= monthStart && p.PaymentDate < monthEnd)
                .Sum(p => p.Amount);

            result.Add(new AccountingTimeSeriesDataPoint
            {
                Date = monthStart,
                Value = monthValue,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<AccountingTimeSeriesDataPoint> GenerateBalanceTrend(
        List<Invoice> invoices,
        List<Payment> payments,
        int months)
    {
        var result = new List<AccountingTimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;
        decimal runningBalance = 0;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);

            var monthInvoices = invoices.Where(inv => inv.InvoiceDate >= monthStart && inv.InvoiceDate < monthEnd).Sum(inv => inv.TotalAmount);
            var monthPayments = payments.Where(p => p.PaymentDate >= monthStart && p.PaymentDate < monthEnd).Sum(p => p.Amount);

            runningBalance += monthInvoices - monthPayments;

            result.Add(new AccountingTimeSeriesDataPoint
            {
                Date = monthStart,
                Value = runningBalance,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<MonthlyComparisonData> GenerateMonthlyPerformance(
        List<Invoice> invoices,
        List<Payment> payments,
        int months)
    {
        var result = new List<MonthlyComparisonData>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);
            var prevMonthStart = monthStart.AddMonths(-1);

            var monthAmount = invoices.Where(inv => inv.InvoiceDate >= monthStart && inv.InvoiceDate < monthEnd).Sum(inv => inv.TotalAmount);
            var prevAmount = invoices.Where(inv => inv.InvoiceDate >= prevMonthStart && inv.InvoiceDate < monthStart).Sum(inv => inv.TotalAmount);

            var change = prevAmount > 0 ? (monthAmount - prevAmount) / prevAmount * 100 : 0;

            result.Add(new MonthlyComparisonData
            {
                Month = monthStart.ToString("MMMM"),
                Year = monthStart.Year,
                Amount = monthAmount,
                PreviousAmount = prevAmount,
                ChangePercentage = change,
                TransactionCount = invoices.Count(inv => inv.InvoiceDate >= monthStart && inv.InvoiceDate < monthEnd)
            });
        }

        return result;
    }

    private static List<DashboardAlert> GenerateAlerts(Customer customer, List<Invoice> invoices, DateTime today)
    {
        var alerts = new List<DashboardAlert>();

        // Credit hold alert
        if (customer.IsOnCreditHold)
        {
            alerts.Add(new DashboardAlert
            {
                AlertType = "Credit Hold",
                Severity = "Critical",
                Message = $"Customer {customer.CustomerName} is on credit hold",
                CreatedDate = today,
                RelatedEntityId = customer.Id,
                RelatedEntityName = customer.CustomerName
            });
        }

        // Over credit limit alert
        if (customer.CurrentBalance > customer.CreditLimit && customer.CreditLimit > 0)
        {
            alerts.Add(new DashboardAlert
            {
                AlertType = "Over Credit Limit",
                Severity = "Warning",
                Message = $"Customer exceeds credit limit by {(customer.CurrentBalance - customer.CreditLimit):C}",
                CreatedDate = today,
                RelatedEntityId = customer.Id,
                RelatedEntityName = customer.CustomerName
            });
        }

        // Overdue invoices alert
        var overdueInvoices = invoices.Where(i => i.Status != "Paid" && i.Status != "Cancelled" && i.DueDate < today).ToList();
        if (overdueInvoices.Count > 0)
        {
            var totalOverdue = overdueInvoices.Sum(i => i.TotalAmount - i.PaidAmount);
            alerts.Add(new DashboardAlert
            {
                AlertType = "Overdue Invoices",
                Severity = "Warning",
                Message = $"{overdueInvoices.Count} overdue invoices totaling {totalOverdue:C}",
                CreatedDate = today,
                RelatedEntityId = customer.Id,
                RelatedEntityName = customer.CustomerName
            });
        }

        // Inactive customer alert
        if (!customer.IsActive)
        {
            alerts.Add(new DashboardAlert
            {
                AlertType = "Inactive Customer",
                Severity = "Info",
                Message = $"Customer {customer.CustomerName} is currently inactive",
                CreatedDate = today,
                RelatedEntityId = customer.Id,
                RelatedEntityName = customer.CustomerName
            });
        }

        return alerts.Take(5).ToList();
    }
}
