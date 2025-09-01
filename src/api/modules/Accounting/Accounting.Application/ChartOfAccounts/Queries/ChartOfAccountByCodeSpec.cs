using Accounting.Domain;
using Ardalis.Specification;
using FSH.Framework.Core.Specifications;

namespace Accounting.Application.ChartOfAccounts.Queries;

public class ChartOfAccountByCodeSpec : Specification<ChartOfAccount>
{
    public ChartOfAccountByCodeSpec(string accountCode)
    {
        Query.Where(c => c.AccountCode == accountCode);
    }
}
