namespace Accounting.Application.AccountingPeriods.Dtos;

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
    public DefaultIdType Id { get; set; } = id;
    public string Name { get; set; } = name;
    public DateTime StartDate { get; set; } = startDate;
    public DateTime EndDate { get; set; } = endDate;
    public bool IsClosed { get; set; } = isClosed;
    public bool IsAdjustmentPeriod { get; set; } = isAdjustmentPeriod;
    public int FiscalYear { get; set; } = fiscalYear;
    public string PeriodType { get; set; } = periodType;
    public string? Description { get; set; } = description;
    public string? Notes { get; set; } = notes;
}
