namespace Accounting.Application.Invoices.Send.v1;

/// <summary>
/// Handler for sending an invoice.
/// Transitions invoice status from Draft to Sent.
/// </summary>
public sealed class SendInvoiceHandler(
    ILogger<SendInvoiceHandler> logger,
    [FromKeyedServices("accounting:invoices")] IRepository<Invoice> repository)
    : IRequestHandler<SendInvoiceCommand, SendInvoiceResponse>
{
    public async Task<SendInvoiceResponse> Handle(SendInvoiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var invoice = await repository.GetByIdAsync(request.InvoiceId, cancellationToken).ConfigureAwait(false);
        if (invoice is null)
        {
            throw new InvoiceNotFoundException(request.InvoiceId);
        }

        invoice.Send();

        await repository.UpdateAsync(invoice, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Invoice {InvoiceId} sent to member {MemberId}", invoice.Id, invoice.MemberId);

        return new SendInvoiceResponse(invoice.Id, invoice.Status);
    }
}

