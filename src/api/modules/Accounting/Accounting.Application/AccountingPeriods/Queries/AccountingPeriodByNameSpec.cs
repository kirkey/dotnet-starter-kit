namespace Accounting.Application.AccountingPeriods.Queries;

public sealed class AccountingPeriodByNameSpec : Ardalis.Specification.Specification<Accounting.Domain.AccountingPeriod>
{
    public AccountingPeriodByNameSpec(string name)
    {
        Query.Where(p => p.Name == name);
    }
}
