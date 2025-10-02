using Accounting.Application.PaymentAllocations.Commands;
using Accounting.Domain.Entities;

namespace Accounting.Application.PaymentAllocations.Handlers;

public class CreatePaymentAllocationHandler(IRepository<PaymentAllocation> repository)
    : IRequestHandler<CreatePaymentAllocationCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreatePaymentAllocationCommand request, CancellationToken cancellationToken)
    {
        var pa = PaymentAllocation.Create(
            request.PaymentId,
            request.InvoiceId,
            request.Amount,
            request.Notes);

        await repository.AddAsync(pa, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return pa.Id;
    }
}
