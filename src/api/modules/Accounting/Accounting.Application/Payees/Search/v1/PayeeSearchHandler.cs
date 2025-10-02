using Accounting.Application.Payees.Get.v1;
using Accounting.Domain.Entities;

namespace Accounting.Application.Payees.Search.v1;

/// <summary>
/// Handler for searching payees with pagination and filtering capabilities.
/// Implements CQRS pattern for query operations with comprehensive search functionality.
/// </summary>
public sealed class PayeeSearchHandler(
    [FromKeyedServices("accounting:payees")] IReadRepository<Payee> repository)
    : IRequestHandler<PayeeSearchCommand, PagedList<PayeeResponse>>
{
    /// <summary>
    /// Handles the search request for payees with filtering and pagination.
    /// </summary>
    /// <param name="request">The search command containing filter criteria and pagination settings.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A paginated list of payee responses matching the search criteria.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    public async Task<PagedList<PayeeResponse>> Handle(PayeeSearchCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new PayeeSearchSpecs(request);

        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<PayeeResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
