using MediatR;
using Accounting.Application.AccountingPeriods.Dtos;

namespace Accounting.Application.AccountingPeriods.Get;

public class GetAccountingPeriodRequest(DefaultIdType id) : IRequest<AccountingPeriodDto>
{
    public DefaultIdType Id { get; set; } = id;
}
