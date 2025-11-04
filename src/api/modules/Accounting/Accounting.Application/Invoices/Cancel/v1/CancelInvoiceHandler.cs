namespace Accounting.Application.Invoices.Cancel.v1;

/// <summary>
/// Handler for cancelling an invoice.
/// </summary>
public sealed class CancelInvoiceHandler(
    ILogger<CancelInvoiceHandler> logger,
    [FromKeyedServices("accounting:invoices")] IRepository<Invoice> repository)
    : IRequestHandler<CancelInvoiceCommand, CancelInvoiceResponse>
{
    public async Task<CancelInvoiceResponse> Handle(CancelInvoiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var invoice = await repository.GetByIdAsync(request.InvoiceId, cancellationToken).ConfigureAwait(false);
        if (invoice is null)
        {
            throw new InvoiceNotFoundException(request.InvoiceId);
        }

        invoice.Cancel(request.Reason);

        await repository.UpdateAsync(invoice, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Invoice {InvoiceId} cancelled. Reason: {Reason}", 
            invoice.Id, request.Reason ?? "Not specified");

        return new CancelInvoiceResponse(invoice.Id, invoice.Status);
    }
}

