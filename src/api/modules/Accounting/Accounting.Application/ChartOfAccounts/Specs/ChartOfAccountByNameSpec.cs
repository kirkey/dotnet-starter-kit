using Accounting.Domain.Entities;

namespace Accounting.Application.ChartOfAccounts.Specs;

public sealed class ChartOfAccountByNameSpec : Specification<ChartOfAccount>, ISingleResultSpecification<ChartOfAccount>
{
    public ChartOfAccountByNameSpec(string name, DefaultIdType? excludeId = null)
    {
        var n = (name ?? string.Empty).Trim();
        Query.Where(a => a.AccountName == n);
        if (excludeId != null)
            Query.Where(a => a.Id != excludeId.Value);
    }
}
