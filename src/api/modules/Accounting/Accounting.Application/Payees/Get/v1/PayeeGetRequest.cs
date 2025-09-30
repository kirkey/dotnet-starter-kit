namespace Accounting.Application.Payees.Get.v1;

/// <summary>
/// Request model for retrieving a specific payee by its unique identifier.
/// Follows the CQRS pattern for query operations.
/// </summary>
/// <param name="id">The unique identifier of the payee to retrieve.</param>
public class PayeeGetRequest(DefaultIdType id) : IRequest<PayeeResponse>
{
    /// <summary>
    /// Gets or sets the unique identifier of the payee to retrieve.
    /// </summary>
    public DefaultIdType Id { get; set; } = id;
}
