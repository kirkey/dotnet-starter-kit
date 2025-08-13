using MediatR;
using Accounting.Application.AccountingPeriods.Dtos;

namespace Accounting.Application.AccountingPeriods.Get;

public class GetAccountingPeriodRequest : IRequest<AccountingPeriodDto>
{
    public DefaultIdType Id { get; set; }

    public GetAccountingPeriodRequest(DefaultIdType id)
    {
        Id = id;
    }
}
