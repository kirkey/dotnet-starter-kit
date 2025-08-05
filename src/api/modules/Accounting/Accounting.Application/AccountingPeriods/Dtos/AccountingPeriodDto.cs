namespace Accounting.Application.AccountingPeriods.Dtos;

public record AccountingPeriodDto(
    DefaultIdType Id,
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    bool IsClosed,
    bool IsAdjustmentPeriod,
    int FiscalYear,
    string PeriodType,
    string? Description,
    string? Notes);
