using Accounting.Application.PaymentAllocations.Queries;
using Accounting.Application.PaymentAllocations.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.PaymentAllocations.Handlers;

/// <summary>
/// Handler for searching payment allocations with filtering and pagination.
/// </summary>
public class SearchPaymentAllocationsHandler(
    [FromKeyedServices("accounting:paymentallocations")] IReadRepository<PaymentAllocation> repository)
    : IRequestHandler<SearchPaymentAllocationsQuery, PagedList<PaymentAllocationResponse>>
{
    /// <summary>
    /// Handles the search payment allocations query.
    /// </summary>
    /// <param name="request">The search query containing filter criteria and pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paged list of payment allocation responses.</returns>
    public async Task<PagedList<PaymentAllocationResponse>> Handle(SearchPaymentAllocationsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchPaymentAllocationsSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<PaymentAllocationResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
