using Accounting.Application.PaymentAllocations.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.PaymentAllocations.Queries;

/// <summary>
/// Specification for searching payment allocations with filtering and pagination.
/// </summary>
public class SearchPaymentAllocationsSpec : EntitiesByPaginationFilterSpec<PaymentAllocation, PaymentAllocationResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchPaymentAllocationsSpec"/> class.
    /// </summary>
    /// <param name="request">The search payment allocations query containing filter criteria and pagination parameters.</param>
    public SearchPaymentAllocationsSpec(SearchPaymentAllocationsQuery request)
        : base(request)
    {
        Query
            .Where(x => x.PaymentId == request.PaymentId!.Value, request.PaymentId.HasValue)
            .Where(x => x.InvoiceId == request.InvoiceId!.Value, request.InvoiceId.HasValue)
            .OrderByDescending(x => x.CreatedOn, !request.HasOrderBy());
    }
}

