namespace Accounting.Application.AccountingPeriods.Dtos;

public class AccountingPeriodDto
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsClosed { get; set; }
    public bool IsAdjustmentPeriod { get; set; }
    public int FiscalYear { get; set; }
    public string PeriodType { get; set; } = null!;
    public string? Description { get; set; }
    public string? Notes { get; set; }

    public AccountingPeriodDto(
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
        Id = id;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        IsClosed = isClosed;
        IsAdjustmentPeriod = isAdjustmentPeriod;
        FiscalYear = fiscalYear;
        PeriodType = periodType;
        Description = description;
        Notes = notes;
    }
}
