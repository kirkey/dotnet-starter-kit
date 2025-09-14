namespace Accounting.Application.AccountingPeriods.Delete;

public class DeleteAccountingPeriodRequest(DefaultIdType id) : IRequest
{
    public DefaultIdType Id { get; set; } = id;
}
