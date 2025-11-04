using Accounting.Application.Invoices.Get.v1;

namespace Accounting.Application.Invoices.Search.v1;

/// <summary>
/// Handler for searching invoices with pagination and filters.
/// </summary>
public sealed class SearchInvoicesHandler(
    [FromKeyedServices("accounting:invoices")] IReadRepository<Invoice> repository)
    : IRequestHandler<SearchInvoicesCommand, PagedList<InvoiceResponse>>
{
    public async Task<PagedList<InvoiceResponse>> Handle(SearchInvoicesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchInvoicesSpec(request);

        var invoices = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var response = invoices.Select(invoice => new InvoiceResponse
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
        }).ToList();

        return new PagedList<InvoiceResponse>(response, request.PageNumber, request.PageSize, totalCount);
    }
}

