namespace Accounting.Application.ChartOfAccounts.Queries;

public sealed class ChartOfAccountByCodeSpec : Specification<ChartOfAccount>, ISingleResultSpecification<ChartOfAccount>
{
    public ChartOfAccountByCodeSpec(string code)
    {
        var c = (code ?? string.Empty).Trim();
        Query.Where(a => a.AccountCode == c);
    }
}
