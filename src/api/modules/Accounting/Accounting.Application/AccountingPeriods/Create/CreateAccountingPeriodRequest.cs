using MediatR;

namespace Accounting.Application.AccountingPeriods.Create;

public record CreateAccountingPeriodRequest(
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    int FiscalYear,
    string PeriodType,
    bool IsAdjustmentPeriod = false,
    string? Description = null,
    string? Notes = null) : IRequest<DefaultIdType>;
