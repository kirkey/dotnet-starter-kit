namespace Accounting.Application.Dashboard;

/// <summary>
/// Represents a single data point in a time series chart.
/// </summary>
public sealed record TimeSeriesDataPoint
{
    public DateTime Date { get; init; }
    public decimal Value { get; init; }
    public string? Label { get; init; }
}

/// <summary>
/// Represents account-based data breakdown for charts.
/// </summary>
public sealed record AccountBreakdown
{
    public Guid AccountId { get; init; }
    public string AccountCode { get; init; } = default!;
    public string AccountName { get; init; } = default!;
    public decimal Amount { get; init; }
    public decimal Percentage { get; init; }
}

/// <summary>
/// Represents period-based data breakdown for charts.
/// </summary>
public sealed record PeriodBreakdown
{
    public string PeriodName { get; init; } = default!;
    public decimal Amount { get; init; }
    public decimal Percentage { get; init; }
    public int TransactionCount { get; init; }
}

/// <summary>
/// Represents monthly comparison data.
/// </summary>
public sealed record MonthlyComparisonData
{
    public string Month { get; init; } = default!;
    public int Year { get; init; }
    public decimal Amount { get; init; }
    public decimal PreviousAmount { get; init; }
    public decimal ChangePercentage { get; init; }
    public int TransactionCount { get; init; }
}

/// <summary>
/// Represents a dashboard alert.
/// </summary>
public sealed record DashboardAlert
{
    public string AlertType { get; init; } = default!;
    public string Severity { get; init; } = default!;
    public string Message { get; init; } = default!;
    public DateTime CreatedDate { get; init; }
    public Guid? RelatedEntityId { get; init; }
    public string? RelatedEntityName { get; init; }
}

/// <summary>
/// Represents a recent transaction for dashboard display.
/// </summary>
public sealed record RecentTransactionInfo
{
    public Guid TransactionId { get; init; }
    public string TransactionType { get; init; } = default!;
    public string ReferenceNumber { get; init; } = default!;
    public DateTime TransactionDate { get; init; }
    public decimal Amount { get; init; }
    public string? Description { get; init; }
    public string? Status { get; init; }
}
