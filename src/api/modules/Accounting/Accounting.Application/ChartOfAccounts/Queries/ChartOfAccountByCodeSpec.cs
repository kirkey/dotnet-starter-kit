using Accounting.Domain;
using Ardalis.Specification;

namespace Accounting.Application.ChartOfAccounts.Queries;

public class ChartOfAccountByCodeSpec : Specification<ChartOfAccount>
{
    public ChartOfAccountByCodeSpec(string accountCode)
    {
        Query.Where(c => c.AccountCode == accountCode);
    }
}
