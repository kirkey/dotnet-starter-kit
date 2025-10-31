namespace Accounting.Application.AccountingPeriods.Responses;

/// <summary>
/// Read model returned for accounting period queries and endpoints.
/// Contains a flattened view of the <see cref="AccountingPeriod"/> aggregate suitable for API responses.
/// </summary>
public class AccountingPeriodResponse(
    DefaultIdType id,
    string name,
    DateTime startDate,
    DateTime endDate,
    bool isClosed,
    bool isAdjustmentPeriod,
    int fiscalYear,
    string periodType,
    string? description,
    string? notes)
{
    /// <summary>
    /// Unique identifier for the accounting period.
    /// </summary>
    public DefaultIdType Id { get; set; } = id;

    /// <summary>
    /// Human-friendly name of the period.
    /// </summary>
    public string Name { get; set; } = name;

    /// <summary>
    /// Inclusive start date of the period.
    /// </summary>
    public DateTime StartDate { get; set; } = startDate;

    /// <summary>
    /// Inclusive end date of the period.
    /// </summary>
    public DateTime EndDate { get; set; } = endDate;

    /// <summary>
    /// Whether the period has been closed to postings.
    /// </summary>
    public bool IsClosed { get; set; } = isClosed;

    /// <summary>
    /// Whether this is an adjustment period (e.g., period 13 for year-end adjustments).
    /// </summary>
    public bool IsAdjustmentPeriod { get; set; } = isAdjustmentPeriod;

    /// <summary>
    /// Fiscal year associated with the period.
    /// </summary>
    public int FiscalYear { get; set; } = fiscalYear;

    /// <summary>
    /// The granularity/type of the period (Monthly, Quarterly, Yearly, Annual).
    /// </summary>
    public string PeriodType { get; set; } = periodType;

    /// <summary>
    /// Optional long description for display or reporting.
    /// </summary>
    public string? Description { get; set; } = description;

    /// <summary>
    /// Optional administrative notes.
    /// </summary>
    public string? Notes { get; set; } = notes;
}
