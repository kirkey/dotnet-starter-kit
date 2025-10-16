using Accounting.Domain.Entities;

namespace Accounting.Application.Banks.Queries;

/// <summary>
/// Specification for finding banks by name (partial match).
/// </summary>
public class BankByNameSpec : Specification<Bank>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankByNameSpec"/> class.
    /// </summary>
    /// <param name="name">The name to search for (partial match).</param>
    public BankByNameSpec(string name)
    {
        Query.Where(x => x.Name.Contains(name));
    }
}
