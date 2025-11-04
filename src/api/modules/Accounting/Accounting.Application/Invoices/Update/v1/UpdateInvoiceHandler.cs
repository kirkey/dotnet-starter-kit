namespace Accounting.Application.Invoices.Update.v1;

/// <summary>
/// Handler for updating an invoice.
/// </summary>
public sealed class UpdateInvoiceHandler(
    ILogger<UpdateInvoiceHandler> logger,
    [FromKeyedServices("accounting:invoices")] IRepository<Invoice> repository)
    : IRequestHandler<UpdateInvoiceCommand, UpdateInvoiceResponse>
{
    public async Task<UpdateInvoiceResponse> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var invoice = await repository.GetByIdAsync(request.InvoiceId, cancellationToken).ConfigureAwait(false);
        if (invoice is null)
        {
            throw new InvoiceNotFoundException(request.InvoiceId);
        }

        invoice.Update(
            dueDate: request.DueDate,
            usageCharge: request.UsageCharge,
            basicServiceCharge: request.BasicServiceCharge,
            taxAmount: request.TaxAmount,
            otherCharges: request.OtherCharges,
            lateFee: request.LateFee,
            reconnectionFee: request.ReconnectionFee,
            depositAmount: request.DepositAmount,
            demandCharge: request.DemandCharge,
            rateSchedule: request.RateSchedule,
            description: request.Description,
            notes: request.Notes);

        await repository.UpdateAsync(invoice, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Invoice {InvoiceId} updated", invoice.Id);

        return new UpdateInvoiceResponse(invoice.Id);
    }
}

