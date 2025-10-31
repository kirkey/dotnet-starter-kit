namespace Accounting.Application.AccountingPeriods.Specs;

/// <summary>
/// Specification that selects an accounting period with an exact name match.
/// </summary>
/// <remarks>
/// Used to detect duplicate names when creating or updating accounting periods.
/// </remarks>
public sealed class AccountingPeriodByNameSpec : Specification<AccountingPeriod>
{
    public AccountingPeriodByNameSpec(string name)
    {
        Query.Where(p => p.Name == name);
    }
}
