namespace FSH.Starter.WebApi.Store.Application.Dashboard;

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
/// Represents category-based data breakdown for charts.
/// </summary>
public sealed record CategoryBreakdown
{
    public string CategoryName { get; init; } = default!;
    public int OrderCount { get; init; }
    public decimal TotalValue { get; init; }
    public decimal Percentage { get; init; }
}
