using Accounting.Domain.Entities;

namespace Accounting.Application.Banks.Queries;

/// <summary>
/// Specification for finding a bank by its unique code.
/// </summary>
public class BankByCodeSpec : Specification<Bank>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankByCodeSpec"/> class.
    /// </summary>
    /// <param name="bankCode">The bank code to search for.</param>
    public BankByCodeSpec(string bankCode)
    {
        Query.Where(x => x.BankCode == bankCode);
    }
}
