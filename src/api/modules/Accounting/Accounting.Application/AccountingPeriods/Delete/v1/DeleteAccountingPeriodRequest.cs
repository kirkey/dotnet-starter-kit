namespace Accounting.Application.AccountingPeriods.Delete.v1;

public class DeleteAccountingPeriodCommand(DefaultIdType id) : IRequest
{
    public DefaultIdType Id { get; set; } = id;
}
