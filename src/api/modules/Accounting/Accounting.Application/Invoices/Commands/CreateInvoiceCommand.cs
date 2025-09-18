namespace Accounting.Application.Invoices.Commands;

public class CreateInvoiceCommand : IRequest<DefaultIdType>
{
    public string InvoiceNumber { get; set; } = default!;
    public DefaultIdType MemberId { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    public DefaultIdType? ConsumptionDataId { get; set; }
    public decimal UsageCharge { get; set; }
    public decimal BasicServiceCharge { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal OtherCharges { get; set; }
    public decimal KWhUsed { get; set; }
    public string BillingPeriod { get; set; } = default!;
    public decimal? LateFee { get; set; }
    public decimal? ReconnectionFee { get; set; }
    public decimal? DepositAmount { get; set; }
    public string? RateSchedule { get; set; }
    public decimal? DemandCharge { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
}