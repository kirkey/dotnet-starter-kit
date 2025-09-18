using Accounting.Application.Invoices.Dtos;
using Accounting.Application.Invoices.Queries;

namespace Accounting.Application.Invoices.Handlers;

public class GetInvoiceByIdHandler(IReadRepository<Invoice> repository)
    : IRequestHandler<GetInvoiceByIdQuery, InvoiceDto>
{
    public async Task<InvoiceDto> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        var invoice = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (invoice == null)
            throw new NotFoundException($"Invoice with Id {request.Id} not found");

        return new InvoiceDto
        {
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            MemberId = invoice.MemberId,
            InvoiceDate = invoice.InvoiceDate,
            DueDate = invoice.DueDate,
            TotalAmount = invoice.TotalAmount,
            PaidAmount = invoice.PaidAmount,
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
            Notes = invoice.Notes
        };
    }
}
