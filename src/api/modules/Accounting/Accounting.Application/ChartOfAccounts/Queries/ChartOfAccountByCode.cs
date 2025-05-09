using Accounting.Application.ChartOfAccounts.Dtos;
using Accounting.Domain;
using Ardalis.Specification;

namespace Accounting.Application.ChartOfAccounts.Queries;

public sealed class ChartOfAccountByCode :
    Specification<ChartOfAccount, ChartOfAccountDto>,
    ISingleResultSpecification<ChartOfAccount, ChartOfAccountDto>
{
    public ChartOfAccountByCode(string accountCode) =>
        Query.Where(w => w.AccountCode == accountCode);
}
