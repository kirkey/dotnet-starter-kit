namespace Accounting.Application.Invoices.Void.v1;

/// <summary>
/// Handler for voiding an invoice.
/// </summary>
public class VoidInvoiceHandlerV1(IRepository<Invoice> repository)
    : IRequestHandler<VoidInvoiceCommand, VoidInvoiceResponse>
{
    public async Task<VoidInvoiceResponse> Handle(VoidInvoiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var invoice = await repository.GetByIdAsync(request.InvoiceId, cancellationToken);
        if (invoice is null)
        {
            throw new InvoiceNotFoundException(request.InvoiceId);
        }

        invoice.Void(request.Reason);

        await repository.UpdateAsync(invoice, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new VoidInvoiceResponse(invoice.Id, invoice.Status);
    }
}

