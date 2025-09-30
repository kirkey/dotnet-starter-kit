namespace Accounting.Application.ChartOfAccounts.Specs;

/// <summary>
/// Specification for finding a Chart of Account by its unique account code.
/// Used for duplicate checking during import and general lookups.
/// </summary>
public sealed class ChartOfAccountByCodeSpec : Specification<ChartOfAccount>
{
    /// <summary>
    /// Initializes the specification with the account code to search for.
    /// </summary>
    /// <param name="accountCode">The account code to find</param>
    public ChartOfAccountByCodeSpec(string accountCode)
    {
        Query.Where(x => x.AccountCode == accountCode);
    }
}
