using Accounting.Application.PaymentAllocations.Dtos;

namespace Accounting.Application.PaymentAllocations.Queries;

public class SearchPaymentAllocationsQuery : IRequest<List<PaymentAllocationDto>>
{
    public DefaultIdType? PaymentId { get; set; }
    public DefaultIdType? InvoiceId { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}

