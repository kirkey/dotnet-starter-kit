namespace Accounting.Application.Invoice.Dtos
{
    public class InvoiceDto
    {
        public DefaultIdType Id { get; set; }
        public string InvoiceNumber { get; set; } = default!;
        public DefaultIdType MemberId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string Status { get; set; } = default!;
        public DefaultIdType? ConsumptionDataId { get; set; }
        public decimal UsageCharge { get; set; }
        public decimal BasicServiceCharge { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal OtherCharges { get; set; }
        public decimal KWhUsed { get; set; }
        public string BillingPeriod { get; set; } = default!;
        public DateTime? PaidDate { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal? LateFee { get; set; }
        public decimal? ReconnectionFee { get; set; }
        public decimal? DepositAmount { get; set; }
        public string? RateSchedule { get; set; }
        public decimal? DemandCharge { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
    }
}

