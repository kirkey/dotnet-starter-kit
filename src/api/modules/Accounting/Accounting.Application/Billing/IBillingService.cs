namespace Accounting.Application.Billing;

public record InvoiceLineDto(string Description, decimal Quantity, decimal UnitPrice, decimal TotalPrice);

public record InvoiceDraft(decimal UsageCharge, decimal FixedCharge, decimal TotalAmount, List<InvoiceLineDto> Lines);

public interface IBillingService
{
    /// <summary>
    /// Calculate invoice draft (line items and totals) for a given consumption record and rate schedule.
    /// Pure calculation (no persistence).
    /// </summary>
    InvoiceDraft CalculateInvoiceDraft(ConsumptionData consumption, RateSchedule rateSchedule);
}

