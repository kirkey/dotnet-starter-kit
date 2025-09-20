using Accounting.Application.PaymentAllocations.Queries;
using Accounting.Application.PaymentAllocations.Responses;

namespace Accounting.Application.PaymentAllocations.Handlers;

public class SearchPaymentAllocationsHandler(IReadRepository<PaymentAllocation> repository)
    : IRequestHandler<SearchPaymentAllocationsQuery, List<PaymentAllocationResponse>>
{
    public async Task<List<PaymentAllocationResponse>> Handle(SearchPaymentAllocationsQuery request, CancellationToken cancellationToken)
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

        return query.Select(x => new PaymentAllocationResponse
        {
            Id = x.Id,
            PaymentId = x.PaymentId,
            InvoiceId = x.InvoiceId,
            Amount = x.Amount,
            Notes = x.Description ?? x.Notes
        }).ToList();
    }
}
