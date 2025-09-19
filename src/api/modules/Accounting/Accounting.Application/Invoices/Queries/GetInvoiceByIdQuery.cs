using Accounting.Application.Invoices.Responses;

namespace Accounting.Application.Invoices.Queries;

public class GetInvoiceByIdQuery : IRequest<InvoiceResponse>
{
    public DefaultIdType Id { get; set; }
}
