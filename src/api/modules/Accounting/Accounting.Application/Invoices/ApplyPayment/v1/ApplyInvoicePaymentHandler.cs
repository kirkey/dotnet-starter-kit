namespace Accounting.Application.Invoices.ApplyPayment.v1;

/// <summary>
/// Handler for applying a partial payment to an invoice.
/// Automatically marks invoice as paid when total payments meet or exceed invoice amount.
/// </summary>
public sealed class ApplyInvoicePaymentHandler(
    ILogger<ApplyInvoicePaymentHandler> logger,
    [FromKeyedServices("accounting:invoices")] IRepository<Invoice> repository)
    : IRequestHandler<ApplyInvoicePaymentCommand, ApplyInvoicePaymentResponse>
{
    public async Task<ApplyInvoicePaymentResponse> Handle(ApplyInvoicePaymentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var invoice = await repository.GetByIdAsync(request.InvoiceId, cancellationToken).ConfigureAwait(false);
        if (invoice is null)
        {
            throw new InvoiceNotFoundException(request.InvoiceId);
        }

        invoice.ApplyPayment(request.Amount, request.PaymentDate, request.PaymentMethod);

        await repository.UpdateAsync(invoice, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Payment of {Amount} applied to invoice {InvoiceId}. Status: {Status}", 
            request.Amount, invoice.Id, invoice.Status);

        return new ApplyInvoicePaymentResponse(
            invoice.Id,
            invoice.Status,
            invoice.TotalAmount,
            invoice.PaidAmount,
            invoice.GetOutstandingAmount()
        );
    }
}

