namespace Accounting.Application.AccountingPeriods.Create.v1;

public record CreateAccountingPeriodRequest(
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    int FiscalYear,
    string PeriodType,
    bool IsAdjustmentPeriod = false,
    string? Description = null,
    string? Notes = null) : IRequest<DefaultIdType>;
