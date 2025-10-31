namespace Accounting.Application.Checks.Queries;

/// <summary>
/// Specification to find checks by bank account code.
/// </summary>
public class ChecksByBankAccountSpec : Specification<Check>
{
    public ChecksByBankAccountSpec(string bankAccountCode)
    {
        Query.Where(c => c.BankAccountCode == bankAccountCode);
    }
}

