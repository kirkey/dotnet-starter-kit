using Accounting.Application.PaymentAllocations.Dtos;
using Accounting.Application.PaymentAllocations.Queries;

namespace Accounting.Application.PaymentAllocations.Handlers;

public class SearchPaymentAllocationsHandler(IReadRepository<PaymentAllocation> repository)
    : IRequestHandler<SearchPaymentAllocationsQuery, List<PaymentAllocationDto>>
{
    public async Task<List<PaymentAllocationDto>> Handle(SearchPaymentAllocationsQuery request, CancellationToken cancellationToken)
    {
        var query = (await repository.ListAsync(cancellationToken)).AsQueryable();

        if (request.PaymentId.HasValue)
            query = query.Where(x => x.PaymentId == request.PaymentId.Value);
        if (request.InvoiceId.HasValue)
            query = query.Where(x => x.InvoiceId == request.InvoiceId.Value);

        if (request.Skip.HasValue)
            query = query.Skip(request.Skip.Value);
        if (request.Take.HasValue)
            query = query.Take(request.Take.Value);

        return query.Select(x => new PaymentAllocationDto
        {
            Id = x.Id,
            PaymentId = x.PaymentId,
            InvoiceId = x.InvoiceId,
            Amount = x.Amount,
            Notes = x.Description ?? x.Notes
        }).ToList();
    }
}

