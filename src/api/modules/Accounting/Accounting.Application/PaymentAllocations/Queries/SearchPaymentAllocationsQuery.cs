using Accounting.Application.PaymentAllocations.Responses;

namespace Accounting.Application.PaymentAllocations.Queries;

public class SearchPaymentAllocationsQuery : IRequest<List<PaymentAllocationResponse>>
{
    public DefaultIdType? PaymentId { get; set; }
    public DefaultIdType? InvoiceId { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}
