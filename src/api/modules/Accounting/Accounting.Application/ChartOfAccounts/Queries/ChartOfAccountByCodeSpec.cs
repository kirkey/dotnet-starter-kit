using Accounting.Application.ChartOfAccounts.Dtos;
using Accounting.Domain;
using Ardalis.Specification;

namespace Accounting.Application.ChartOfAccounts.Queries;

public sealed class ChartOfAccountByCodeSpec :
    Specification<ChartOfAccount, ChartOfAccountDto>,
    ISingleResultSpecification<ChartOfAccount, ChartOfAccountDto>
{
    public ChartOfAccountByCodeSpec(string accountCode) =>
        Query.Where(w => w.AccountCode == accountCode);
}
