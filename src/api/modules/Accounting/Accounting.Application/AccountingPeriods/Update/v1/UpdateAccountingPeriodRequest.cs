namespace Accounting.Application.AccountingPeriods.Update.v1;

public record UpdateAccountingPeriodCommand(
    DefaultIdType Id,
    string? Name = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    bool IsAdjustmentPeriod = false,
    int? FiscalYear = null,
    string? PeriodType = null,
    string? Description = null,
    string? Notes = null) : IRequest<DefaultIdType>;
