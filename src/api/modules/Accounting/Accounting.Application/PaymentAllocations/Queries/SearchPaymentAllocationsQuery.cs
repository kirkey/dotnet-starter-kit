using Accounting.Application.PaymentAllocations.Responses;

namespace Accounting.Application.PaymentAllocations.Queries;

/// <summary>
/// Query to search payment allocations with filtering and pagination.
/// </summary>
public class SearchPaymentAllocationsQuery : PaginationFilter, IRequest<PagedList<PaymentAllocationResponse>>
{
    /// <summary>
    /// Gets or sets the payment ID to filter by.
    /// </summary>
    public DefaultIdType? PaymentId { get; set; }

    /// <summary>
    /// Gets or sets the invoice ID to filter by.
    /// </summary>
    public DefaultIdType? InvoiceId { get; set; }
}
