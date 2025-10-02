using Accounting.Application.PaymentAllocations.Commands;
using Accounting.Domain.Entities;

namespace Accounting.Application.PaymentAllocations.Handlers;

public class UpdatePaymentAllocationHandler(IRepository<PaymentAllocation> repository)
    : IRequestHandler<UpdatePaymentAllocationCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdatePaymentAllocationCommand request, CancellationToken cancellationToken)
    {
        var existingAllocation = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = existingAllocation ?? throw new PaymentAllocationByIdNotFoundException(request.Id);

        // "Update" is a deleted and create operation to respect domain immutability
        await repository.DeleteAsync(existingAllocation, cancellationToken);

        var newAllocation = PaymentAllocation.Create(
            existingAllocation.PaymentId,
            existingAllocation.InvoiceId,
            request.Amount ?? existingAllocation.Amount,
            request.Notes
            );

        await repository.AddAsync(newAllocation, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return newAllocation.Id;
    }
}
