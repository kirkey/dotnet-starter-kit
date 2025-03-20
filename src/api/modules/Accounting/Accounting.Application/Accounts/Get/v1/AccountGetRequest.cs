using Accounting.Application.Accounts.Dtos;
using MediatR;

namespace Accounting.Application.Accounts.Get.v1;
public class AccountGetRequest(DefaultIdType id) : IRequest<AccountDto>
{
    public DefaultIdType Id { get; set; } = id;
}
