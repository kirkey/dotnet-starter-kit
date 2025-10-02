using Accounting.Application.PaymentAllocations.Commands;
using Accounting.Domain.Entities;

namespace Accounting.Application.PaymentAllocations.Handlers;

public class DeletePaymentAllocationHandler(IRepository<PaymentAllocation> repository)
    : IRequestHandler<DeletePaymentAllocationCommand, Unit>
{
    public async Task<Unit> Handle(DeletePaymentAllocationCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = entity ?? throw new PaymentAllocationByIdNotFoundException(request.Id);

        await repository.DeleteAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
