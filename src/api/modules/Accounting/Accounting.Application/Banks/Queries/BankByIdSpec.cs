using Accounting.Domain.Entities;

namespace Accounting.Application.Banks.Queries;

/// <summary>
/// Specification for finding a bank by its ID.
/// </summary>
public class BankByIdSpec : Specification<Bank>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankByIdSpec"/> class.
    /// </summary>
    /// <param name="id">The bank ID to search for.</param>
    public BankByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}

