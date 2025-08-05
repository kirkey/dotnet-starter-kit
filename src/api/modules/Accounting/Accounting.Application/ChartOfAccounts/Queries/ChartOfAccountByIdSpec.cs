using Accounting.Application.ChartOfAccounts.Dtos;
using Accounting.Domain;
using Ardalis.Specification;

namespace Accounting.Application.ChartOfAccounts.Queries;

public sealed class ChartOfAccountByIdSpec :
    Specification<ChartOfAccount, ChartOfAccountDto>,
    ISingleResultSpecification<ChartOfAccount, ChartOfAccountDto>
{
    public ChartOfAccountByIdSpec(DefaultIdType id) =>
        Query.Where(w => w.Id == id);
}
