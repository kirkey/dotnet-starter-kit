namespace Accounting.Application.AccountingPeriods.Update;

public record UpdateAccountingPeriodRequest(
    DefaultIdType Id,
    string? Name = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    bool? IsAdjustmentPeriod = null,
    int? FiscalYear = null,
    string? PeriodType = null,
    string? Description = null,
    string? Notes = null) : IRequest<DefaultIdType>;
