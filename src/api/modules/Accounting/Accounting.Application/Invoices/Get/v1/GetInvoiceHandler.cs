namespace Accounting.Application.Invoices.Get.v1;

/// <summary>
/// Handler for getting a specific invoice by ID.
/// </summary>
public sealed class GetInvoiceHandler(
    [FromKeyedServices("accounting:invoices")] IReadRepository<Invoice> repository)
    : IRequestHandler<GetInvoiceRequest, InvoiceResponse>
{
    /// <summary>
    /// Handles the retrieval of an invoice by its ID.
    /// </summary>
    /// <param name="request">The get invoice request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The invoice response.</returns>
    /// <exception cref="InvoiceNotFoundException">Thrown when the invoice is not found.</exception>
    public async Task<InvoiceResponse> Handle(GetInvoiceRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var invoice = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (invoice is null)
        {
            throw new InvoiceNotFoundException(request.Id);
        }

        return new InvoiceResponse
        {
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            MemberId = invoice.MemberId,
            InvoiceDate = invoice.InvoiceDate,
            DueDate = invoice.DueDate,
            TotalAmount = invoice.TotalAmount,
            PaidAmount = invoice.PaidAmount,
            OutstandingAmount = invoice.GetOutstandingAmount(),
            Status = invoice.Status,
            ConsumptionId = invoice.ConsumptionId,
            UsageCharge = invoice.UsageCharge,
            BasicServiceCharge = invoice.BasicServiceCharge,
            TaxAmount = invoice.TaxAmount,
            OtherCharges = invoice.OtherCharges,
            KWhUsed = invoice.KWhUsed,
            BillingPeriod = invoice.BillingPeriod,
            PaidDate = invoice.PaidDate,
            PaymentMethod = invoice.PaymentMethod,
            LateFee = invoice.LateFee,
            ReconnectionFee = invoice.ReconnectionFee,
            DepositAmount = invoice.DepositAmount,
            RateSchedule = invoice.RateSchedule,
            DemandCharge = invoice.DemandCharge,
            Description = invoice.Description,
            Notes = invoice.Notes,
            LineItemCount = invoice.LineItems.Count,
            CreatedOn = invoice.CreatedOn,
            LastModifiedOn = invoice.LastModifiedOn
        };
    }
}

