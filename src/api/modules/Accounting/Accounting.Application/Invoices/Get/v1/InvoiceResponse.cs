namespace Accounting.Application.Invoices.Get.v1;

/// <summary>
/// Response model representing an invoice entity.
/// Contains comprehensive invoice information including billing details, charges, and payment status.
/// </summary>
public sealed class InvoiceResponse
{
    /// <summary>
    /// Unique identifier for the invoice.
    /// </summary>
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// Unique invoice number for reference and tracking.
    /// </summary>
    public string InvoiceNumber { get; set; } = null!;
    
    /// <summary>
    /// Member or customer identifier associated with this invoice.
    /// </summary>
    public DefaultIdType MemberId { get; set; }
    
    /// <summary>
    /// Date when the invoice was generated.
    /// </summary>
    public DateTime InvoiceDate { get; set; }
    
    /// <summary>
    /// Due date for payment of this invoice.
    /// </summary>
    public DateTime DueDate { get; set; }
    
    /// <summary>
    /// Total amount due on this invoice.
    /// </summary>
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// Amount already paid towards this invoice.
    /// </summary>
    public decimal PaidAmount { get; set; }
    
    /// <summary>
    /// Current status of the invoice (e.g., "Pending", "Paid", "Overdue").
    /// </summary>
    public string Status { get; set; } = null!;
    
    /// <summary>
    /// Reference to the consumption record for utility billing.
    /// </summary>
    public DefaultIdType? ConsumptionId { get; set; }
    
    /// <summary>
    /// Usage-based charges for the billing period.
    /// </summary>
    public decimal UsageCharge { get; set; }
    
    /// <summary>
    /// Fixed basic service charge for the billing period.
    /// </summary>
    public decimal BasicServiceCharge { get; set; }
    
    /// <summary>
    /// Tax amount applied to this invoice.
    /// </summary>
    public decimal TaxAmount { get; set; }
    
    /// <summary>
    /// Additional charges beyond usage and basic service.
    /// </summary>
    public decimal OtherCharges { get; set; }
    
    /// <summary>
    /// Kilowatt hours consumed during the billing period.
    /// </summary>
    public decimal KWhUsed { get; set; }
    
    /// <summary>
    /// Billing period description (e.g., "Jan 2024", "Q1 2024").
    /// </summary>
    public string BillingPeriod { get; set; } = null!;
    
    /// <summary>
    /// Date when the invoice was paid (if applicable).
    /// </summary>
    public DateTime? PaidDate { get; set; }
    
    /// <summary>
    /// Method used for payment (e.g., "Cash", "Check", "Credit Card").
    /// </summary>
    public string? PaymentMethod { get; set; }
    
    /// <summary>
    /// Late fee applied for overdue payment.
    /// </summary>
    public decimal? LateFee { get; set; }
    
    /// <summary>
    /// Fee charged for service reconnection.
    /// </summary>
    public decimal? ReconnectionFee { get; set; }
    
    /// <summary>
    /// Security deposit amount if applicable.
    /// </summary>
    public decimal? DepositAmount { get; set; }
    
    /// <summary>
    /// Rate schedule applied for billing calculation.
    /// </summary>
    public string? RateSchedule { get; set; }
    
    /// <summary>
    /// Demand charge for peak usage periods.
    /// </summary>
    public decimal? DemandCharge { get; set; }
    
    /// <summary>
    /// Description or additional details about the invoice.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Additional notes or comments about the invoice.
    /// </summary>
    public string? Notes { get; set; }
    
    /// <summary>
    /// Outstanding amount still due (TotalAmount - PaidAmount).
    /// </summary>
    public decimal OutstandingAmount { get; set; }
    
    /// <summary>
    /// Number of line items associated with this invoice.
    /// </summary>
    public int LineItemCount { get; set; }
    
    /// <summary>
    /// Date when the invoice record was created.
    /// </summary>
    public DateTimeOffset? CreatedOn { get; set; }
    
    /// <summary>
    /// Date when the invoice record was last modified.
    /// </summary>
    public DateTimeOffset? LastModifiedOn { get; set; }
}

