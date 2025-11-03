using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.Accounting.Bills;

/// <summary>
/// View model for creating and editing bills.
/// </summary>
public class BillViewModel
{
    /// <summary>
    /// Bill identifier (null for new bills).
    /// </summary>
    public DefaultIdType? Id { get; set; }

    /// <summary>
    /// Bill number or vendor invoice number.
    /// </summary>
    [Required(ErrorMessage = "Bill number is required")]
    [StringLength(50, ErrorMessage = "Bill number cannot exceed 50 characters")]
    public string BillNumber { get; set; } = string.Empty;

    /// <summary>
    /// Vendor identifier.
    /// </summary>
    [Required(ErrorMessage = "Vendor is required")]
    public DefaultIdType? VendorId { get; set; }

    /// <summary>
    /// Vendor name (for display).
    /// </summary>
    public string? VendorName { get; set; }

    /// <summary>
    /// Date bill was issued by vendor.
    /// </summary>
    [Required(ErrorMessage = "Bill date is required")]
    public DateTime? BillDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Date payment is due.
    /// </summary>
    [Required(ErrorMessage = "Due date is required")]
    public DateTime? DueDate { get; set; } = DateTime.Today.AddDays(30);

    /// <summary>
    /// Total amount of the bill.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Current status of the bill.
    /// </summary>
    public string Status { get; set; } = "Draft";

    /// <summary>
    /// Whether bill is posted to GL.
    /// </summary>
    public bool IsPosted { get; set; }

    /// <summary>
    /// Whether bill has been paid.
    /// </summary>
    public bool IsPaid { get; set; }

    /// <summary>
    /// Date bill was paid.
    /// </summary>
    public DateTime? PaidDate { get; set; }

    /// <summary>
    /// Approval status.
    /// </summary>
    public string ApprovalStatus { get; set; } = "Pending";

    /// <summary>
    /// User who approved/rejected the bill.
    /// </summary>
    public string? ApprovedBy { get; set; }

    /// <summary>
    /// Date/time of approval/rejection.
    /// </summary>
    public DateTime? ApprovedDate { get; set; }

    /// <summary>
    /// Optional accounting period.
    /// </summary>
    public DefaultIdType? PeriodId { get; set; }

    /// <summary>
    /// Payment terms.
    /// </summary>
    [StringLength(100, ErrorMessage = "Payment terms cannot exceed 100 characters")]
    public string? PaymentTerms { get; set; }

    /// <summary>
    /// Purchase order number reference.
    /// </summary>
    [StringLength(50, ErrorMessage = "PO number cannot exceed 50 characters")]
    public string? PurchaseOrderNumber { get; set; }

    /// <summary>
    /// Description of the bill.
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
    [Required(ErrorMessage = "At least one line item is required")]
    [MinLength(1, ErrorMessage = "At least one line item is required")]
    public List<BillLineItemViewModel> LineItems { get; set; } = new();

    /// <summary>
    /// Subtotal amount (sum of all line amounts excluding tax).
    /// </summary>
    public decimal SubtotalAmount => LineItems.Sum(l => l.Amount);

    /// <summary>
    /// Total tax amount across all line items.
    /// </summary>
    public decimal TotalTaxAmount => LineItems.Sum(l => l.TaxAmount);

    /// <summary>
    /// Calculated total from line items (subtotal + tax).
    /// </summary>
    public decimal CalculatedTotal => SubtotalAmount + TotalTaxAmount;
}

/// <summary>
/// View model for bill line items.
/// </summary>
public class BillLineItemViewModel
{
    /// <summary>
    /// Line item identifier (null for new items).
    /// </summary>
    public DefaultIdType? Id { get; set; }

    /// <summary>
    /// Parent bill identifier.
    /// </summary>
    public DefaultIdType? BillId { get; set; }

    /// <summary>
    /// Line number for ordering.
    /// </summary>
    [Required(ErrorMessage = "Line number is required")]
    [Range(1, 9999, ErrorMessage = "Line number must be between 1 and 9999")]
    public int LineNumber { get; set; }

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
    /// Extended line amount.
    /// </summary>
    [Required(ErrorMessage = "Amount is required")]
    [Range(0, 999999999, ErrorMessage = "Amount cannot be negative")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Chart of account for GL posting.
    /// </summary>
    [Required(ErrorMessage = "Account is required")]
    public DefaultIdType? ChartOfAccountId { get; set; }

    /// <summary>
    /// Chart of account code (for display).
    /// </summary>
    public string? ChartOfAccountCode { get; set; }

    /// <summary>
    /// Chart of account name (for display).
    /// </summary>
    public string? ChartOfAccountName { get; set; }

    /// <summary>
    /// Optional tax code.
    /// </summary>
    public DefaultIdType? TaxCodeId { get; set; }

    /// <summary>
    /// Tax code name (for display).
    /// </summary>
    public string? TaxCodeName { get; set; }

    /// <summary>
    /// Tax amount for this line.
    /// </summary>
    [Range(0, 999999999, ErrorMessage = "Tax amount cannot be negative")]
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// Optional project reference.
    /// </summary>
    public DefaultIdType? ProjectId { get; set; }

    /// <summary>
    /// Project name (for display).
    /// </summary>
    public string? ProjectName { get; set; }

    /// <summary>
    /// Optional cost center reference.
    /// </summary>
    public DefaultIdType? CostCenterId { get; set; }

    /// <summary>
    /// Cost center name (for display).
    /// </summary>
    public string? CostCenterName { get; set; }

    /// <summary>
    /// Additional notes for this line.
    /// </summary>
    [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
    public string? Notes { get; set; }

    /// <summary>
    /// Calculate amount from quantity and unit price.
    /// </summary>
    public void CalculateAmount()
    {
        Amount = Quantity * UnitPrice;
    }
}

