using Accounting.Application.FiscalPeriodCloses.Queries;

namespace Accounting.Application.FiscalPeriodCloses.Get;

/// <summary>
/// Request to get complete fiscal period close details including tasks and validation status.
/// </summary>
public record GetFiscalPeriodCloseRequest(DefaultIdType Id) : IRequest<FiscalPeriodCloseDetailsDto>;

