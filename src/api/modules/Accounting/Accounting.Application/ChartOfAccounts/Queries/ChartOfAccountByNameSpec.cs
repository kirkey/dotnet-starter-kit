namespace Accounting.Application.ChartOfAccounts.Queries;

public sealed class ChartOfAccountByNameSpec : Ardalis.Specification.Specification<Accounting.Domain.ChartOfAccount>, Ardalis.Specification.ISingleResultSpecification<Accounting.Domain.ChartOfAccount>
{
    public ChartOfAccountByNameSpec(string name, DefaultIdType? excludeId = null)
    {
        var n = (name ?? string.Empty).Trim();
        Query.Where(a => a.AccountName == n);
        if (excludeId != null)
            Query.Where(a => a.Id != excludeId.Value);
    }
}

