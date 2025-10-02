using Accounting.Domain.Entities;

namespace Accounting.Application.Payees.Specs;

/// <summary>
/// Specification for finding a Payee by its unique payee code.
/// Used for duplicate checking during import and general lookups.
/// </summary>
public sealed class PayeeByCodeSpec : Specification<Payee>
{
    /// <summary>
    /// Initializes the specification with the payee code to search for.
    /// </summary>
    /// <param name="payeeCode">The payee code to find</param>
    public PayeeByCodeSpec(string payeeCode)
    {
        Query.Where(x => x.PayeeCode == payeeCode);
    }
}
