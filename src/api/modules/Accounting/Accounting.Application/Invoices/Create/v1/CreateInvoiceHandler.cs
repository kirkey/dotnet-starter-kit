namespace Accounting.Application.Invoices.Create.v1;

/// <summary>
/// Handler for creating a new invoice in the accounting system.
/// </summary>
public sealed class CreateInvoiceHandler(
    ILogger<CreateInvoiceHandler> logger,
    [FromKeyedServices("accounting:invoices")] IRepository<Invoice> repository)
    : IRequestHandler<CreateInvoiceCommand, CreateInvoiceResponse>
{
    /// <summary>
    /// Handles the creation of a new invoice.
    /// </summary>
    /// <param name="request">The create invoice command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response containing the created invoice ID.</returns>
    public async Task<CreateInvoiceResponse> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var invoice = Invoice.Create(
            request.InvoiceNumber,
            request.MemberId,
            request.InvoiceDate,
            request.DueDate,
            request.ConsumptionId,
            request.UsageCharge,
            request.BasicServiceCharge,
            request.TaxAmount,
            request.OtherCharges,
            request.KWhUsed,
            request.BillingPeriod,
            request.LateFee,
            request.ReconnectionFee,
            request.DepositAmount,
            request.RateSchedule,
            request.DemandCharge,
            request.Description,
            request.Notes);

        await repository.AddAsync(invoice, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Invoice created {InvoiceId} for member {MemberId}", invoice.Id, invoice.MemberId);
        
        return new CreateInvoiceResponse(invoice.Id);
    }
}

