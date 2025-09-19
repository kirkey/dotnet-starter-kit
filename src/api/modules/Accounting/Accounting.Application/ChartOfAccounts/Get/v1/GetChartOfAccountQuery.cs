using Accounting.Application.ChartOfAccounts.Responses;

namespace Accounting.Application.ChartOfAccounts.Get.v1;
public class GetChartOfAccountQuery(DefaultIdType id) : IRequest<ChartOfAccountResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
