using Accounting.Domain.Entities;

namespace Accounting.Application.Payees.Specs;

/// <summary>
/// Specification for finding a Payee by its Tax Identification Number (TIN).
/// Used for duplicate checking during import to prevent TIN conflicts.
/// </summary>
public sealed class PayeeByTinSpec : Specification<Payee>
{
    /// <summary>
    /// Initializes the specification with the TIN to search for.
    /// </summary>
    /// <param name="tin">The Tax Identification Number to find</param>
    public PayeeByTinSpec(string tin)
    {
        Query.Where(x => x.Tin == tin);
    }
}
