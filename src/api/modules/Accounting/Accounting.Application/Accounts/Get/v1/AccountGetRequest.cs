using MediatR;

namespace Accounting.Application.Accounts.Get.v1;
public class AccountGetRequest(DefaultIdType id) : IRequest<AccountResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
