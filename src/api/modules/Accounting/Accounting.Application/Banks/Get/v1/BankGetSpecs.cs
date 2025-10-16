using Accounting.Domain.Entities;

namespace Accounting.Application.Banks.Get.v1;

/// <summary>
/// Specification for retrieving bank details by ID.
/// </summary>
public class BankGetSpecs : Specification<Bank>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankGetSpecs"/> class.
    /// </summary>
    /// <param name="id">The bank ID to retrieve.</param>
    public BankGetSpecs(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}

