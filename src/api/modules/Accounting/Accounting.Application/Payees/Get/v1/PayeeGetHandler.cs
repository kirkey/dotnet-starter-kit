using Accounting.Domain.Entities;

namespace Accounting.Application.Payees.Get.v1;

/// <summary>
/// Handler for retrieving a specific payee by its unique identifier.
/// Implements caching for improved performance and follows CQRS pattern.
/// </summary>
public sealed class PayeeGetHandler(
    [FromKeyedServices("accounting:payees")] IReadRepository<Payee> repository,
    ICacheService cache)
    : IRequestHandler<PayeeGetRequest, PayeeResponse>
{
    /// <summary>
    /// Handles the request to retrieve a payee by ID with caching support.
    /// </summary>
    /// <param name="request">The get payee request containing the ID.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>The payee response if found.</returns>
    /// <exception cref="PayeeNotFoundException">Thrown when the payee is not found.</exception>
    public async Task<PayeeResponse> Handle(PayeeGetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"payee:{request.Id}",
            async () =>
            {
                var spec = new PayeeGetSpecs(request.Id);
                var response = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false) ??
                               throw new PayeeNotFoundException(request.Id);
                return response;
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
