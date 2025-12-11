using Accounting.Application.Dashboard;

namespace Accounting.Application.Customers.Dashboard;

/// <summary>
/// Response containing comprehensive customer performance metrics and analytics.
/// </summary>
public sealed record CustomerDashboardResponse
{
    // Basic Customer Info
    public Guid CustomerId { get; init; }
    public string CustomerNumber { get; init; } = default!;
    public string CustomerName { get; init; } = default!;
    public string CustomerType { get; init; } = default!;
    public string? ContactName { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string BillingAddress { get; init; } = default!;
    public bool IsActive { get; init; }
    public string Status { get; init; } = default!;
    public bool IsOnCreditHold { get; init; }
    public string PaymentTerms { get; init; } = default!;

    // Credit Overview
    public CustomerCreditMetrics Credit { get; init; } = new();

    // Invoice Metrics
    public CustomerInvoiceMetrics Invoices { get; init; } = new();

    // Payment Metrics
    public CustomerPaymentMetrics Payments { get; init; } = new();

    // Aging Analysis
    public CustomerAgingMetrics Aging { get; init; } = new();

    // Trend Data for Charts
    public List<TimeSeriesDataPoint> InvoiceValueTrend { get; init; } = [];
    public List<TimeSeriesDataPoint> PaymentTrend { get; init; } = [];
    public List<TimeSeriesDataPoint> BalanceTrend { get; init; } = [];

    // Recent Invoices
    public List<RecentInvoiceInfo> RecentInvoices { get; init; } = [];

    // Recent Payments
    public List<RecentCustomerPaymentInfo> RecentPayments { get; init; } = [];

    // Monthly Performance
    public List<MonthlyComparisonData> MonthlyPerformance { get; init; } = [];

    // Alerts
    public List<DashboardAlert> Alerts { get; init; } = [];
}

public sealed record CustomerCreditMetrics
{
    public decimal CreditLimit { get; init; }
    public decimal CurrentBalance { get; init; }
    public decimal AvailableCredit { get; init; }
    public decimal CreditUtilization { get; init; }
    public decimal OverdueBalance { get; init; }
    public decimal DiscountPercentage { get; init; }
    public bool TaxExempt { get; init; }
}

public sealed record CustomerInvoiceMetrics
{
    public int TotalInvoices { get; init; }
    public int TotalInvoicesYTD { get; init; }
    public decimal TotalInvoicedAmount { get; init; }
    public decimal TotalInvoicedAmountYTD { get; init; }
    public decimal TotalInvoicedAmountLastYear { get; init; }
    public int DraftInvoices { get; init; }
    public int SentInvoices { get; init; }
    public int PaidInvoices { get; init; }
    public int OverdueInvoices { get; init; }
    public decimal AverageInvoiceAmount { get; init; }
    public decimal GrowthPercentage { get; init; }
}

public sealed record CustomerPaymentMetrics
{
    public int TotalPayments { get; init; }
    public int TotalPaymentsYTD { get; init; }
    public decimal TotalPaymentAmount { get; init; }
    public decimal TotalPaymentAmountYTD { get; init; }
    public decimal AveragePaymentAmount { get; init; }
    public decimal AveragePaymentDays { get; init; }
    public decimal OnTimePaymentRate { get; init; }
    public int PaymentsThisMonth { get; init; }
    public decimal PaymentsThisMonthAmount { get; init; }
}

public sealed record CustomerAgingMetrics
{
    public decimal Current { get; init; }
    public decimal Days1To30 { get; init; }
    public decimal Days31To60 { get; init; }
    public decimal Days61To90 { get; init; }
    public decimal Over90Days { get; init; }
    public decimal TotalOutstanding { get; init; }
    public int DaysSalesOutstanding { get; init; }
}

public sealed record RecentInvoiceInfo
{
    public Guid InvoiceId { get; init; }
    public string InvoiceNumber { get; init; } = default!;
    public DateTime InvoiceDate { get; init; }
    public DateTime DueDate { get; init; }
    public decimal TotalAmount { get; init; }
    public decimal PaidAmount { get; init; }
    public decimal BalanceDue { get; init; }
    public string Status { get; init; } = default!;
}

public sealed record RecentCustomerPaymentInfo
{
    public Guid PaymentId { get; init; }
    public string PaymentNumber { get; init; } = default!;
    public DateTime PaymentDate { get; init; }
    public decimal Amount { get; init; }
    public string PaymentMethod { get; init; } = default!;
}
