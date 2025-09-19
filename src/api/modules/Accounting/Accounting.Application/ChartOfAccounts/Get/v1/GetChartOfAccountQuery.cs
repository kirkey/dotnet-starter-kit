using Accounting.Application.ChartOfAccounts.Dtos;

namespace Accounting.Application.ChartOfAccounts.Get.v1;
public class GetChartOfAccountQuery(DefaultIdType id) : IRequest<ChartOfAccountResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
