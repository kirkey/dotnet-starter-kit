namespace Accounting.Application.Payees.Queries;

/// <summary>
/// Specification for finding a payee by its unique identifier.
/// Used in Get operations to retrieve a specific payee entity.
/// </summary>
public class PayeeByIdSpec : Specification<Payee>
{
    /// <summary>
    /// Initializes a new instance of the PayeeByIdSpec class.
    /// </summary>
    /// <param name="id">The unique identifier of the payee to find.</param>
    public PayeeByIdSpec(DefaultIdType id)
    {
        Query.Where(p => p.Id == id);
    }
}
