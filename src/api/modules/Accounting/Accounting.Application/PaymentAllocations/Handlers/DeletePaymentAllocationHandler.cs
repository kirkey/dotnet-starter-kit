using Accounting.Application.PaymentAllocations.Commands;

namespace Accounting.Application.PaymentAllocations.Handlers;

public class DeletePaymentAllocationHandler(IRepository<PaymentAllocation> repository)
    : IRequestHandler<DeletePaymentAllocationCommand, Unit>
{
    public async Task<Unit> Handle(DeletePaymentAllocationCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            throw new NotFoundException($"PaymentAllocation with Id {request.Id} not found");

        await repository.DeleteAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
