using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.Accounting.Invoices;

/// <summary>
/// View model for creating and editing invoices.
/// </summary>
public class InvoiceViewModel
{
    /// <summary>
    /// Invoice identifier (null for new invoices).
    /// </summary>
    public DefaultIdType? Id { get; set; }

    /// <summary>
    /// Unique invoice number for reference and tracking.
    /// </summary>
    [Required(ErrorMessage = "Invoice number is required")]
    [StringLength(50, ErrorMessage = "Invoice number cannot exceed 50 characters")]
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Member or customer identifier associated with this invoice.
    /// </summary>
    [Required(ErrorMessage = "Member is required")]
    public DefaultIdType? MemberId { get; set; }

    /// <summary>
    /// Member name (for display).
    /// </summary>
    public string? MemberName { get; set; }

    /// <summary>
    /// Date when the invoice was generated.
    /// </summary>
    [Required(ErrorMessage = "Invoice date is required")]
    public DateTime? InvoiceDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Due date for payment of this invoice.
    /// </summary>
    [Required(ErrorMessage = "Due date is required")]
    public DateTime? DueDate { get; set; } = DateTime.Today.AddDays(30);

    /// <summary>
    /// Total amount of the invoice.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Amount already paid towards this invoice.
    /// </summary>
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// Current status of the invoice.
    /// </summary>
    public string Status { get; set; } = "Draft";

    /// <summary>
    /// Reference to the consumption record for utility billing.
    /// </summary>
    public DefaultIdType? ConsumptionId { get; set; }

    /// <summary>
    /// Usage-based charges for the billing period.
    /// </summary>
    [Range(0, 999999999, ErrorMessage = "Usage charge cannot be negative")]
    public decimal UsageCharge { get; set; }

    /// <summary>
    /// Fixed basic service charge for the billing period.
    /// </summary>
    [Range(0, 999999999, ErrorMessage = "Basic service charge cannot be negative")]
    public decimal BasicServiceCharge { get; set; }

    /// <summary>
    /// Tax amount applied to this invoice.
    /// </summary>
    [Range(0, 999999999, ErrorMessage = "Tax amount cannot be negative")]
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// Additional charges beyond usage and basic service.
    /// </summary>
    [Range(0, 999999999, ErrorMessage = "Other charges cannot be negative")]
    public decimal OtherCharges { get; set; }

    /// <summary>
    /// Kilowatt hours consumed during the billing period.
    /// </summary>
    [Range(0, 999999999, ErrorMessage = "KWh used cannot be negative")]
    public decimal KWhUsed { get; set; }

    /// <summary>
    /// Billing period description (e.g., "Jan 2024", "Q1 2024").
    /// </summary>
    [Required(ErrorMessage = "Billing period is required")]
    [StringLength(50, ErrorMessage = "Billing period cannot exceed 50 characters")]
    public string BillingPeriod { get; set; } = string.Empty;

    /// <summary>
    /// Date when the invoice was paid (if applicable).
    /// </summary>
    public DateTime? PaidDate { get; set; }

    /// <summary>
    /// Method used for payment (e.g., "Cash", "Check", "Credit Card").
    /// </summary>
    [StringLength(50, ErrorMessage = "Payment method cannot exceed 50 characters")]
    public string? PaymentMethod { get; set; }

    /// <summary>
    /// Late fee applied for overdue payment.
    /// </summary>
    [Range(0, 999999999, ErrorMessage = "Late fee cannot be negative")]
    public decimal? LateFee { get; set; }

    /// <summary>
    /// Fee charged for service reconnection.
    /// </summary>
    [Range(0, 999999999, ErrorMessage = "Reconnection fee cannot be negative")]
    public decimal? ReconnectionFee { get; set; }

    /// <summary>
    /// Security deposit amount.
    /// </summary>
    [Range(0, 999999999, ErrorMessage = "Deposit amount cannot be negative")]
    public decimal? DepositAmount { get; set; }

    /// <summary>
    /// Rate schedule applied to this invoice.
    /// </summary>
    [StringLength(100, ErrorMessage = "Rate schedule cannot exceed 100 characters")]
    public string? RateSchedule { get; set; }

    /// <summary>
    /// Demand charge for peak power usage.
    /// </summary>
    [Range(0, 999999999, ErrorMessage = "Demand charge cannot be negative")]
    public decimal? DemandCharge { get; set; }

    /// <summary>
    /// Description of the invoice.
    /// </summary>
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    [StringLength(2000, ErrorMessage = "Notes cannot exceed 2000 characters")]
    public string? Notes { get; set; }

    /// <summary>
    /// Collection of line items.
    /// </summary>
    public List<InvoiceLineItemViewModel> LineItems { get; set; } = new();

    /// <summary>
    /// Calculated total from charges (usage + basic service + tax + other charges + fees).
    /// </summary>
    public decimal CalculatedTotal => UsageCharge + BasicServiceCharge + TaxAmount + OtherCharges 
        + (LateFee ?? 0) + (ReconnectionFee ?? 0) + (DepositAmount ?? 0) + (DemandCharge ?? 0)
        + LineItems.Sum(l => l.TotalPrice);

    /// <summary>
    /// Outstanding balance (total - paid).
    /// </summary>
    public decimal OutstandingBalance => TotalAmount - PaidAmount;
}

/// <summary>
/// View model for invoice line items.
/// </summary>
public class InvoiceLineItemViewModel
{
    /// <summary>
    /// Line item identifier (null for new items).
    /// </summary>
    public DefaultIdType? Id { get; set; }

    /// <summary>
    /// Parent invoice identifier.
    /// </summary>
    public DefaultIdType? InvoiceId { get; set; }

    /// <summary>
    /// Description of goods/services.
    /// </summary>
    [Required(ErrorMessage = "Description is required")]
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Quantity of items.
    /// </summary>
    [Required(ErrorMessage = "Quantity is required")]
    [Range(0.0001, 999999999, ErrorMessage = "Quantity must be positive")]
    public decimal Quantity { get; set; } = 1;

    /// <summary>
    /// Price per unit.
    /// </summary>
    [Required(ErrorMessage = "Unit price is required")]
    [Range(0, 999999999, ErrorMessage = "Unit price cannot be negative")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Total line price (Quantity × UnitPrice).
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Chart of account for GL posting.
    /// </summary>
    public DefaultIdType? AccountId { get; set; }

    /// <summary>
    /// Chart of account code (for display).
    /// </summary>
    public string? AccountCode { get; set; }

    /// <summary>
    /// Chart of account name (for display).
    /// </summary>
    public string? AccountName { get; set; }

    /// <summary>
    /// Calculates the total price based on quantity and unit price.
    /// </summary>
    public void CalculateTotal()
    {
        TotalPrice = Quantity * UnitPrice;
    }
}

