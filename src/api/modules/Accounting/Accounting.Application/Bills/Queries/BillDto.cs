namespace Accounting.Application.Bills.Queries;

/// <summary>
/// DTO for bill query responses.
/// </summary>
public sealed record BillDto
{
    public DefaultIdType Id { get; init; }
    public string BillNumber { get; init; } = string.Empty;
    public DefaultIdType VendorId { get; init; }
    public string? VendorName { get; init; }
    public DateTime BillDate { get; init; }
    public DateTime DueDate { get; init; }
    public decimal TotalAmount { get; init; }
    public string Status { get; init; } = string.Empty;
    public bool IsPosted { get; init; }
    public bool IsPaid { get; init; }
    public DateTime? PaidDate { get; init; }
    public string ApprovalStatus { get; init; } = string.Empty;
    public string? ApprovedBy { get; init; }
    public DateTime? ApprovedDate { get; init; }
    public DefaultIdType? PeriodId { get; init; }
    public string? PaymentTerms { get; init; }
    public string? PurchaseOrderNumber { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public List<BillLineItemDto>? LineItems { get; init; }
    public DateTime CreatedOn { get; init; }
    public string? CreatedBy { get; init; }
    public DateTime? LastModifiedOn { get; init; }
    public string? LastModifiedBy { get; init; }
}

/// <summary>
/// DTO for bill line item query responses.
/// </summary>
public sealed record BillLineItemDto
{
    public DefaultIdType Id { get; init; }
    public DefaultIdType BillId { get; init; }
    public int LineNumber { get; init; }
    public string Description { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal Amount { get; init; }
    public DefaultIdType ChartOfAccountId { get; init; }
    public string? ChartOfAccountCode { get; init; }
    public string? ChartOfAccountName { get; init; }
    public DefaultIdType? TaxCodeId { get; init; }
    public string? TaxCodeName { get; init; }
    public decimal TaxAmount { get; init; }
    public DefaultIdType? ProjectId { get; init; }
    public string? ProjectName { get; init; }
    public DefaultIdType? CostCenterId { get; init; }
    public string? CostCenterName { get; init; }
    public string? Notes { get; init; }
}

/// <summary>
/// DTO for bill search responses.
/// </summary>
public sealed record BillSearchResponse
{
    public DefaultIdType Id { get; init; }
    public string BillNumber { get; init; } = string.Empty;
    public DefaultIdType VendorId { get; init; }
    public string? VendorName { get; init; }
    public DateTime BillDate { get; init; }
    public DateTime DueDate { get; init; }
    public decimal TotalAmount { get; init; }
    public string Status { get; init; } = string.Empty;
    public bool IsPosted { get; init; }
    public bool IsPaid { get; init; }
    public DateTime? PaidDate { get; init; }
    public string ApprovalStatus { get; init; } = string.Empty;
    public string? ApprovedBy { get; init; }
    public DefaultIdType? PeriodId { get; init; }
    public string? PaymentTerms { get; init; }
    public string? PurchaseOrderNumber { get; init; }
    public int LineItemCount { get; init; }
}

