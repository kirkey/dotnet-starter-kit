namespace FSH.Starter.WebApi.Store.Application.Dashboard;

/// <summary>
/// Represents a single data point in a time series chart for Store dashboards.
/// </summary>
public sealed record StoreTimeSeriesDataPoint
{
    public DateTime Date { get; init; }
    public decimal Value { get; init; }
    public string? Label { get; init; }
}

/// <summary>
/// Represents category-based data breakdown for charts in Store dashboards.
/// </summary>
public sealed record StoreCategoryBreakdown
{
    public string CategoryName { get; init; } = default!;
    public int OrderCount { get; init; }
    public decimal TotalValue { get; init; }
    public decimal Percentage { get; init; }
}
