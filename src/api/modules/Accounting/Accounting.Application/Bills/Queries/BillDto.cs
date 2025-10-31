namespace Accounting.Application.Bills.Queries;

/// <summary>
/// Bill data transfer object for list views.
/// </summary>
public record BillDto
{
    public DefaultIdType Id { get; init; }
    public string BillNumber { get; init; } = string.Empty;
    public DefaultIdType VendorId { get; init; }
    public string VendorInvoiceNumber { get; init; } = string.Empty;
    public DateTime BillDate { get; init; }
    public DateTime DueDate { get; init; }
    public decimal TotalAmount { get; init; }
    public decimal PaidAmount { get; init; }
    public decimal OutstandingAmount { get; init; }
    public string Status { get; init; } = string.Empty;
    public bool IsApproved { get; init; }
    public bool IsOverdue { get; init; }
    public int DaysPastDue { get; init; }
    public bool IsDiscountAvailable { get; init; }
}

/// <summary>
/// Bill data transfer object for detail view with all properties and line items.
/// </summary>
public record BillDetailsDto : BillDto
{
    public decimal SubtotalAmount { get; init; }
    public decimal TaxAmount { get; init; }
    public decimal ShippingAmount { get; init; }
    public decimal DiscountAmount { get; init; }
    public DateTime? DiscountDate { get; init; }
    public string? PaymentTerms { get; init; }
    public DateTime? ApprovalDate { get; init; }
    public string? ApprovedBy { get; init; }
    public DateTime? PaidDate { get; init; }
    public string? PaymentMethod { get; init; }
    public string? PaymentReference { get; init; }
    public DefaultIdType? PurchaseOrderId { get; init; }
    public DefaultIdType? ExpenseAccountId { get; init; }
    public DefaultIdType? PeriodId { get; init; }
    public string? RejectionReason { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public List<BillLineItemDto> LineItems { get; init; } = new();
}

/// <summary>
/// Bill line item data transfer object.
/// </summary>
public record BillLineItemDto
{
    public string Description { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal LineTotal { get; init; }
    public DefaultIdType? AccountId { get; init; }
}

