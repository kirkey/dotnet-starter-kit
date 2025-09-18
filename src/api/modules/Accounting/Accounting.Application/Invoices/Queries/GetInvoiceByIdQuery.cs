using Accounting.Application.Invoices.Dtos;

namespace Accounting.Application.Invoices.Queries;

public class GetInvoiceByIdQuery : IRequest<InvoiceDto>
{
    public DefaultIdType Id { get; set; }
}