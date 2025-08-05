using MediatR;

namespace Accounting.Application.AccountingPeriods.Delete;

public record DeleteAccountingPeriodRequest(DefaultIdType Id) : IRequest;
