using Accounting.Application.Dashboard;

namespace Accounting.Application.Vendors.Dashboard;

/// <summary>
/// Response containing comprehensive vendor performance metrics and analytics.
/// </summary>
public sealed record VendorDashboardResponse
{
    // Basic Vendor Info
    public Guid VendorId { get; init; }
    public string VendorCode { get; init; } = default!;
    public string VendorName { get; init; } = default!;
    public string? ContactPerson { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
    public bool IsActive { get; init; }
    public string? Terms { get; init; }

    // Financial Overview
    public VendorFinancialMetrics Financials { get; init; } = new();

    // Bill Metrics
    public VendorBillMetrics Bills { get; init; } = new();

    // Payment Metrics
    public VendorPaymentMetrics Payments { get; init; } = new();

    // Account Analysis
    public VendorAccountMetrics Accounts { get; init; } = new();

    // Trend Data for Charts
    public List<AccountingTimeSeriesDataPoint> BillValueTrend { get; init; } = [];
    public List<AccountingTimeSeriesDataPoint> PaymentTrend { get; init; } = [];
    public List<AccountingTimeSeriesDataPoint> BalanceTrend { get; init; } = [];

    // Expense Account Breakdown
    public List<AccountBreakdown> ExpenseBreakdown { get; init; } = [];

    // Recent Bills
    public List<RecentBillInfo> RecentBills { get; init; } = [];

    // Recent Payments
    public List<RecentPaymentInfo> RecentPayments { get; init; } = [];

    // Monthly Performance
    public List<MonthlyComparisonData> MonthlyPerformance { get; init; } = [];

    // Alerts
    public List<DashboardAlert> Alerts { get; init; } = [];
}

public sealed record VendorFinancialMetrics
{
    public decimal TotalBillsAmount { get; init; }
    public decimal TotalBillsAmountYTD { get; init; }
    public decimal TotalBillsAmountLastYear { get; init; }
    public decimal TotalPaymentsAmount { get; init; }
    public decimal TotalPaymentsAmountYTD { get; init; }
    public decimal OutstandingBalance { get; init; }
    public decimal OverdueBalance { get; init; }
    public decimal AverageBillAmount { get; init; }
    public decimal GrowthPercentage { get; init; }
}

public sealed record VendorBillMetrics
{
    public int TotalBills { get; init; }
    public int TotalBillsYTD { get; init; }
    public int DraftBills { get; init; }
    public int PendingBills { get; init; }
    public int ApprovedBills { get; init; }
    public int PostedBills { get; init; }
    public int PaidBills { get; init; }
    public int OverdueBills { get; init; }
    public decimal AveragePaymentDays { get; init; }
    public decimal OnTimePaymentRate { get; init; }
}

public sealed record VendorPaymentMetrics
{
    public int TotalPayments { get; init; }
    public int TotalPaymentsYTD { get; init; }
    public int CheckPayments { get; init; }
    public int AchPayments { get; init; }
    public int WirePayments { get; init; }
    public decimal AveragePaymentAmount { get; init; }
    public int PaymentsThisMonth { get; init; }
    public decimal PaymentsThisMonthAmount { get; init; }
}

public sealed record VendorAccountMetrics
{
    public string? DefaultExpenseAccount { get; init; }
    public string? DefaultExpenseAccountName { get; init; }
    public int UniqueExpenseAccounts { get; init; }
    public decimal TotalByDefaultAccount { get; init; }
}

public sealed record RecentBillInfo
{
    public Guid BillId { get; init; }
    public string BillNumber { get; init; } = default!;
    public DateTime BillDate { get; init; }
    public DateTime DueDate { get; init; }
    public decimal TotalAmount { get; init; }
    public string Status { get; init; } = default!;
    public bool IsPosted { get; init; }
    public bool IsPaid { get; init; }
}

public sealed record RecentPaymentInfo
{
    public Guid PaymentId { get; init; }
    public string PaymentNumber { get; init; } = default!;
    public DateTime PaymentDate { get; init; }
    public decimal Amount { get; init; }
    public string PaymentMethod { get; init; } = default!;
    public string? Status { get; init; }
}
