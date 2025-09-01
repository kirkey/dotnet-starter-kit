using Accounting.Domain;
using Ardalis.Specification;
using FSH.Framework.Core.Specifications;

namespace Accounting.Application.ChartOfAccounts.Queries;

public class ChartOfAccountByNameSpec : Specification<ChartOfAccount>
{
    public ChartOfAccountByNameSpec(string accountName)
    {
        Query.Where(c => c.AccountName == accountName);
    }
}
