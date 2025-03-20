using Accounting.Application.Accounts.Dtos;
using Accounting.Domain;
using Ardalis.Specification;

namespace Accounting.Application.Accounts.Queries;

public sealed class AccountByCode :
    Specification<Account, AccountDto>,
    ISingleResultSpecification<Account, AccountDto>
{
    public AccountByCode(string code) =>
        Query.Where(w => w.Code == code);
}
