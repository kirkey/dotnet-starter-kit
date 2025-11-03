namespace Accounting.Application.Bills.Get.v1;

/// <summary>
/// Response containing bill details.
/// </summary>
public sealed record BillResponse
{
    /// <summary>
    /// The unique identifier of the bill.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Unique bill or vendor invoice number.
    /// </summary>
    public string BillNumber { get; init; } = string.Empty;

    /// <summary>
    /// Vendor who issued the bill.
    /// </summary>
    public DefaultIdType VendorId { get; init; }

    /// <summary>
    /// Vendor name.
    /// </summary>
    public string? VendorName { get; init; }

    /// <summary>
    /// Date the bill was issued.
    /// </summary>
    public DateTime BillDate { get; init; }

    /// <summary>
    /// Date payment is due.
    /// </summary>
    public DateTime DueDate { get; init; }

    /// <summary>
    /// Total amount of the bill.
    /// </summary>
    public decimal TotalAmount { get; init; }

    /// <summary>
    /// Current status of the bill.
    /// </summary>
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Indicates if the bill has been posted to the general ledger.
    /// </summary>
    public bool IsPosted { get; init; }

    /// <summary>
    /// Indicates if the bill has been paid.
    /// </summary>
    public bool IsPaid { get; init; }

    /// <summary>
    /// Date the bill was paid.
    /// </summary>
    public DateTime? PaidDate { get; init; }

    /// <summary>
    /// Approval status of the bill.
    /// </summary>
    public string ApprovalStatus { get; init; } = string.Empty;

    /// <summary>
    /// User who approved the bill.
    /// </summary>
    public string? ApprovedBy { get; init; }

    /// <summary>
    /// Date the bill was approved.
    /// </summary>
    public DateTime? ApprovedDate { get; init; }

    /// <summary>
    /// Optional accounting period ID.
    /// </summary>
    public DefaultIdType? PeriodId { get; init; }

    /// <summary>
    /// Optional payment terms.
    /// </summary>
    public string? PaymentTerms { get; init; }

    /// <summary>
    /// Optional purchase order reference.
    /// </summary>
    public string? PurchaseOrderNumber { get; init; }

    /// <summary>
    /// Description of the bill.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Optional notes.
    /// </summary>
    public string? Notes { get; init; }

    /// <summary>
    /// Line items for the bill.
    /// </summary>
    public List<BillLineItemResponse>? LineItems { get; init; }

    /// <summary>
    /// Date and time when the bill was created.
    /// </summary>
    public DateTime CreatedOn { get; init; }

    /// <summary>
    /// User who created the bill.
    /// </summary>
    public string? CreatedBy { get; init; }

    /// <summary>
    /// Date and time when the bill was last modified.
    /// </summary>
    public DateTime? LastModifiedOn { get; init; }

    /// <summary>
    /// User who last modified the bill.
    /// </summary>
    public string? LastModifiedBy { get; init; }
}

/// <summary>
/// Response containing bill line item details.
/// </summary>
public sealed record BillLineItemResponse
{
    /// <summary>
    /// The unique identifier of the line item.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// The unique identifier of the bill.
    /// </summary>
    public DefaultIdType BillId { get; init; }

    /// <summary>
    /// Sequential line number.
    /// </summary>
    public int LineNumber { get; init; }

    /// <summary>
    /// Description of the line item.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Quantity of items or units.
    /// </summary>
    public decimal Quantity { get; init; }

    /// <summary>
    /// Unit price per item.
    /// </summary>
    public decimal UnitPrice { get; init; }

    /// <summary>
    /// Total amount (Quantity * UnitPrice).
    /// </summary>
    public decimal Amount { get; init; }

    /// <summary>
    /// Chart of account to which this line item is posted.
    /// </summary>
    public DefaultIdType ChartOfAccountId { get; init; }

    /// <summary>
    /// Code of the chart of account.
    /// </summary>
    public string? ChartOfAccountCode { get; init; }

    /// <summary>
    /// Name of the chart of account.
    /// </summary>
    public string? ChartOfAccountName { get; init; }

    /// <summary>
    /// Optional tax code identifier.
    /// </summary>
    public DefaultIdType? TaxCodeId { get; init; }

    /// <summary>
    /// Name of the tax code.
    /// </summary>
    public string? TaxCodeName { get; init; }

    /// <summary>
    /// Tax amount applied to this line item.
    /// </summary>
    public decimal TaxAmount { get; init; }

    /// <summary>
    /// Optional project identifier.
    /// </summary>
    public DefaultIdType? ProjectId { get; init; }

    /// <summary>
    /// Name of the project.
    /// </summary>
    public string? ProjectName { get; init; }

    /// <summary>
    /// Optional cost center identifier.
    /// </summary>
    public DefaultIdType? CostCenterId { get; init; }

    /// <summary>
    /// Name of the cost center.
    /// </summary>
    public string? CostCenterName { get; init; }

    /// <summary>
    /// Optional notes for this line item.
    /// </summary>
    public string? Notes { get; init; }
}

