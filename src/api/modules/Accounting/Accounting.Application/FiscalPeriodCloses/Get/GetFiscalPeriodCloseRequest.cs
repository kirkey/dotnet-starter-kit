using Accounting.Application.FiscalPeriodCloses.Responses;

namespace Accounting.Application.FiscalPeriodCloses.Get;

public record GetFiscalPeriodCloseRequest(DefaultIdType Id) : IRequest<FiscalPeriodCloseResponse>;

