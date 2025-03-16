using Accounting.Application.Accounts.Get.v1;
using Accounting.Domain;
using Ardalis.Specification;

namespace Accounting.Application.Accounts.Queries;

public sealed class AccountById :
    Specification<Account, AccountDto>,
    ISingleResultSpecification<Account, AccountDto>
{
    public AccountById(DefaultIdType id) =>
        Query.Where(w => w.Id == id);
}
