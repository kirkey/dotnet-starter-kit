using Accounting.Application.AccountingPeriods.Dtos;

namespace Accounting.Application.AccountingPeriods.Get.v1;

public class GetAccountingPeriodQuery(DefaultIdType id) : IRequest<AccountingPeriodResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
