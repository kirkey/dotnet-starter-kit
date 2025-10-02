using Accounting.Application.PaymentAllocations.Queries;
using Accounting.Application.PaymentAllocations.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.PaymentAllocations.Handlers;

public class GetPaymentAllocationByIdHandler(IReadRepository<PaymentAllocation> repository)
    : IRequestHandler<GetPaymentAllocationByIdQuery, PaymentAllocationResponse>
{
    public async Task<PaymentAllocationResponse> Handle(GetPaymentAllocationByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) throw new PaymentAllocationByIdNotFoundException(request.Id);

        return new PaymentAllocationResponse
        {
            Id = entity.Id,
            PaymentId = entity.PaymentId,
            InvoiceId = entity.InvoiceId,
            Amount = entity.Amount,
            Notes = entity.Description ?? entity.Notes
        };
    }
}
