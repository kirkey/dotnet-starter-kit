namespace Accounting.Application.Invoices.Create.v1;

/// <summary>
/// Command to create a new invoice in the accounting system.
/// </summary>
public sealed record CreateInvoiceCommand : IRequest<CreateInvoiceResponse>
{
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
    /// Reference to the consumption record for utility billing.
    /// </summary>
    public DefaultIdType? ConsumptionId { get; set; }
    
    /// <summary>
    /// Charge based on actual usage (kWh consumed).
    /// </summary>
    public decimal UsageCharge { get; set; }
    
    /// <summary>
    /// Fixed monthly service charge.
    /// </summary>
    public decimal BasicServiceCharge { get; set; }
    
    /// <summary>
    /// Tax amount applied to the invoice.
    /// </summary>
    public decimal TaxAmount { get; set; }
    
    /// <summary>
    /// Any additional charges or fees.
    /// </summary>
    public decimal OtherCharges { get; set; }
    
    /// <summary>
    /// Kilowatt-hours consumed during the billing period.
    /// </summary>
    public decimal KWhUsed { get; set; }
    
    /// <summary>
    /// Billing period (e.g., "January 2024", "Q1 2024").
    /// </summary>
    public string BillingPeriod { get; set; } = null!;
    
    /// <summary>
    /// Late payment fee, if applicable.
    /// </summary>
    public decimal? LateFee { get; set; }
    
    /// <summary>
    /// Reconnection fee for service restoration.
    /// </summary>
    public decimal? ReconnectionFee { get; set; }
    
    /// <summary>
    /// Security deposit amount.
    /// </summary>
    public decimal? DepositAmount { get; set; }
    
    /// <summary>
    /// Rate schedule applied to this invoice.
    /// </summary>
    public string? RateSchedule { get; set; }
    
    /// <summary>
    /// Demand charge for peak power usage.
    /// </summary>
    public decimal? DemandCharge { get; set; }
    
    /// <summary>
    /// Additional description or notes for the invoice.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Internal notes or comments.
    /// </summary>
    public string? Notes { get; set; }
}

