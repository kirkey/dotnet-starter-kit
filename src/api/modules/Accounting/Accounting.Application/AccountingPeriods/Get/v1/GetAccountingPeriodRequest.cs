using Accounting.Application.AccountingPeriods.Dtos;

namespace Accounting.Application.AccountingPeriods.Get.v1;

public class GetAccountingPeriodRequest(DefaultIdType id) : IRequest<AccountingPeriodDto>
{
    public DefaultIdType Id { get; set; } = id;
}
