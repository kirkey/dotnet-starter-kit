using MediatR;
using Accounting.Application.AccountingPeriods.Dtos;

namespace Accounting.Application.AccountingPeriods.Get;

public record GetAccountingPeriodRequest(DefaultIdType Id) : IRequest<AccountingPeriodDto>;
