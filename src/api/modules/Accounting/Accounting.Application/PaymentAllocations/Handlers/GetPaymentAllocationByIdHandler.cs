using Accounting.Application.PaymentAllocations.Dtos;
using Accounting.Application.PaymentAllocations.Queries;

namespace Accounting.Application.PaymentAllocations.Handlers;

public class GetPaymentAllocationByIdHandler(IReadRepository<PaymentAllocation> repository)
    : IRequestHandler<GetPaymentAllocationByIdQuery, PaymentAllocationDto>
{
    public async Task<PaymentAllocationDto> Handle(GetPaymentAllocationByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            throw new NotFoundException($"PaymentAllocation with Id {request.Id} not found");

        return new PaymentAllocationDto
        {
            Id = entity.Id,
            PaymentId = entity.PaymentId,
            InvoiceId = entity.InvoiceId,
            Amount = entity.Amount,
            Notes = entity.Description ?? entity.Notes
        };
    }
}

