using Accounting.Application.ChartOfAccounts.Dtos;
using Accounting.Domain;
using Ardalis.Specification;

namespace Accounting.Application.ChartOfAccounts.Queries;

public sealed class ChartOfAccountByNameSpec :
    Specification<ChartOfAccount, ChartOfAccountDto>,
    ISingleResultSpecification<ChartOfAccount, ChartOfAccountDto>
{
    public ChartOfAccountByNameSpec(string name) =>
        Query.Where(w => w.Name == name);
}
