using Accounting.Application.ChartOfAccounts.Dtos;

namespace Accounting.Application.ChartOfAccounts.Get.v1;
public class GetChartOfAccountRequest : IRequest<ChartOfAccountDto>
{
    public DefaultIdType Id { get; set; }
    public GetChartOfAccountRequest(DefaultIdType id) => Id = id;
}
