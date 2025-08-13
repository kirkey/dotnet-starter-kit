using MediatR;

namespace Accounting.Application.AccountingPeriods.Delete;

public class DeleteAccountingPeriodRequest : IRequest
{
    public DefaultIdType Id { get; set; }

    public DeleteAccountingPeriodRequest(DefaultIdType id)
    {
        Id = id;
    }
}
