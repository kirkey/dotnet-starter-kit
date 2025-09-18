using Accounting.Application.PaymentAllocations.Dtos;

namespace Accounting.Application.PaymentAllocations.Queries;

public class GetPaymentAllocationByIdQuery : IRequest<PaymentAllocationDto>
{
    public DefaultIdType Id { get; set; }
}

