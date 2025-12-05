namespace Accounting.Application.FiscalPeriodCloses.Create.v1;

/// <summary>
/// Command to create a new fiscal period close process.
/// </summary>
public record CreateFiscalPeriodCloseCommand(
    string CloseNumber,
    DefaultIdType PeriodId,
    string CloseType,
    DateTime PeriodStartDate,
    DateTime PeriodEndDate,
    string InitiatedBy,
    string? Description = null,
    string? Notes = null
) : IRequest<FiscalPeriodCloseCreateResponse>;

