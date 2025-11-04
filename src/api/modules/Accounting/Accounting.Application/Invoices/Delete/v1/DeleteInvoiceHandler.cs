namespace Accounting.Application.Invoices.Delete.v1;

/// <summary>
/// Handler for deleting an invoice.
/// Invoices can only be deleted if they are in Draft or Cancelled status.
/// </summary>
public sealed class DeleteInvoiceHandler(
    ILogger<DeleteInvoiceHandler> logger,
    [FromKeyedServices("accounting:invoices")] IRepository<Invoice> repository)
    : IRequestHandler<DeleteInvoiceCommand, DeleteInvoiceResponse>
{
    public async Task<DeleteInvoiceResponse> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var invoice = await repository.GetByIdAsync(request.InvoiceId, cancellationToken).ConfigureAwait(false);
        if (invoice is null)
        {
            throw new InvoiceNotFoundException(request.InvoiceId);
        }

        // Only allow deletion of Draft or Cancelled invoices
        if (invoice.Status != "Draft" && invoice.Status != "Cancelled")
        {
            throw new InvalidOperationException($"Cannot delete invoice with status {invoice.Status}. Only Draft or Cancelled invoices can be deleted.");
        }

        await repository.DeleteAsync(invoice, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Invoice {InvoiceId} deleted", request.InvoiceId);

        return new DeleteInvoiceResponse(request.InvoiceId, true);
    }
}
