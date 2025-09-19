using Accounting.Application.PaymentAllocations.Responses;

namespace Accounting.Application.PaymentAllocations.Queries;

public class GetPaymentAllocationByIdQuery : IRequest<PaymentAllocationResponse>
{
    public DefaultIdType Id { get; set; }
}
