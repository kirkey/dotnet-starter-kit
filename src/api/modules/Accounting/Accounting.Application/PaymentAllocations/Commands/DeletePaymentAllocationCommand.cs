namespace Accounting.Application.PaymentAllocations.Commands;

public class DeletePaymentAllocationCommand : IRequest<Unit>
{
    public DefaultIdType Id { get; set; }
}
