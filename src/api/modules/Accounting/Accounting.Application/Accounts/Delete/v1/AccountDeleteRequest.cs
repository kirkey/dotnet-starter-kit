using MediatR;

namespace Accounting.Application.Accounts.Delete.v1;

public class AccountDeleteRequest(DefaultIdType id) : IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; } = id;
}
