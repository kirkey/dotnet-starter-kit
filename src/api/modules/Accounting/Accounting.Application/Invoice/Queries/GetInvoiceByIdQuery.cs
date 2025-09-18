using Accounting.Application.Invoice.Dtos;

namespace Accounting.Application.Invoice.Queries
{
    public class GetInvoiceByIdQuery : IRequest<InvoiceDto>
    {
        public DefaultIdType Id { get; set; }
    }
}

