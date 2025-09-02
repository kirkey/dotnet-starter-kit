using Accounting.Domain;
using Ardalis.Specification;

namespace Accounting.Application.ChartOfAccounts.Queries;

public class ChartOfAccountByNameSpec : Specification<ChartOfAccount>
{
    public ChartOfAccountByNameSpec(string accountName)
    {
        Query.Where(c => c.AccountName == accountName);
    }
}
