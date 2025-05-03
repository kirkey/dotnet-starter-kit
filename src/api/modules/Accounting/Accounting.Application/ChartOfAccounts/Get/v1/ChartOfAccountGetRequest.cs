using Accounting.Application.ChartOfAccounts.Dtos;
using MediatR;

namespace Accounting.Application.ChartOfAccounts.Get.v1;
public class ChartOfAccountGetRequest(DefaultIdType id) : IRequest<ChartOfAccountDto>
{
    public DefaultIdType Id { get; set; } = id;
}
