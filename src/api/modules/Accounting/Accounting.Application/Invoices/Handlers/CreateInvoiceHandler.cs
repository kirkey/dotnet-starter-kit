using Accounting.Application.Invoices.Commands;

namespace Accounting.Application.Invoices.Handlers;

public class CreateInvoiceHandler(IRepository<Invoice> repository)
    : IRequestHandler<CreateInvoiceCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = Invoice.Create(
            request.InvoiceNumber,
            request.MemberId,
            request.InvoiceDate,
            request.DueDate,
            request.ConsumptionDataId,
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

        await repository.AddAsync(invoice, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return invoice.Id;
    }
}
