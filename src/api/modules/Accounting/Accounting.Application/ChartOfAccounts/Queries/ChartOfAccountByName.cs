using Accounting.Application.ChartOfAccounts.Dtos;
using Accounting.Domain;
using Ardalis.Specification;

namespace Accounting.Application.ChartOfAccounts.Queries;

public sealed class ChartOfAccountByName :
    Specification<ChartOfAccount, ChartOfAccountDto>,
    ISingleResultSpecification<ChartOfAccount, ChartOfAccountDto>
{
    public ChartOfAccountByName(string name) =>
        Query.Where(w => w.Name == name);
}
