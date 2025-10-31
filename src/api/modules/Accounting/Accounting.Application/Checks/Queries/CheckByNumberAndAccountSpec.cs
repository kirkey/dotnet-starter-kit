namespace Accounting.Application.Checks.Queries;

/// <summary>
/// Specification to find a check by check number and bank account.
/// </summary>
public class CheckByNumberAndAccountSpec : Specification<Check>, ISingleResultSpecification<Check>
{
    public CheckByNumberAndAccountSpec(string checkNumber, string bankAccountCode)
    {
        Query.Where(c => c.CheckNumber == checkNumber && c.BankAccountCode == bankAccountCode);
    }
}

