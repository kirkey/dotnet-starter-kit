using Accounting.Application.Invoices.Commands;

namespace Accounting.Application.Invoices.Handlers;

/// <summary>
/// Handler for creating a new invoice.
/// </summary>
public sealed class CreateInvoiceHandler(
    ILogger<CreateInvoiceHandler> logger,
    [FromKeyedServices("accounting:invoices")] IRepository<Invoice> repository)
    : IRequestHandler<CreateInvoiceCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
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
        
        return invoice.Id;
    }
}
