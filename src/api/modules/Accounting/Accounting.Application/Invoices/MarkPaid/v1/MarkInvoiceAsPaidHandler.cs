namespace Accounting.Application.Invoices.MarkPaid.v1;

/// <summary>
/// Handler for marking an invoice as fully paid.
/// </summary>
public sealed class MarkInvoiceAsPaidHandler(
    ILogger<MarkInvoiceAsPaidHandler> logger,
    [FromKeyedServices("accounting:invoices")] IRepository<Invoice> repository)
    : IRequestHandler<MarkInvoiceAsPaidCommand, MarkInvoiceAsPaidResponse>
{
    public async Task<MarkInvoiceAsPaidResponse> Handle(MarkInvoiceAsPaidCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var invoice = await repository.GetByIdAsync(request.InvoiceId, cancellationToken).ConfigureAwait(false);
        if (invoice is null)
        {
            throw new InvoiceNotFoundException(request.InvoiceId);
        }

        invoice.MarkPaid(request.PaidDate, request.PaymentMethod);

        await repository.UpdateAsync(invoice, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Invoice {InvoiceId} marked as paid. Amount: {Amount}", 
            invoice.Id, invoice.TotalAmount);

        return new MarkInvoiceAsPaidResponse(
            invoice.Id,
            invoice.Status,
            invoice.PaidAmount,
            invoice.PaidDate
        );
    }
}

