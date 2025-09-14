namespace Accounting.Application.AccountingPeriods.Queries;

public sealed class AccountingPeriodByNameSpec : Specification<AccountingPeriod>
{
    public AccountingPeriodByNameSpec(string name)
    {
        Query.Where(p => p.Name == name);
    }
}
