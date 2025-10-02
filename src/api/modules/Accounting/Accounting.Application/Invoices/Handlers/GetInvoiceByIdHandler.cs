using Accounting.Application.Invoices.Queries;
using Accounting.Application.Invoices.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.Invoices.Handlers;

public class GetInvoiceByIdHandler(IReadRepository<Invoice> repository)
    : IRequestHandler<GetInvoiceByIdQuery, InvoiceResponse>
{
    public async Task<InvoiceResponse> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        var invoice = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (invoice == null) throw new InvoiceByIdNotFoundException(request.Id);

        return invoice.Adapt<InvoiceResponse>();
    }
}
