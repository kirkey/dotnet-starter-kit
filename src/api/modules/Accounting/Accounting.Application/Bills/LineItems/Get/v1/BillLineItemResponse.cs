namespace Accounting.Application.Bills.LineItems.Get.v1;

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
    /// The ID of the bill this line item belongs to.
    /// </summary>
    public DefaultIdType BillId { get; init; }

    /// <summary>
    /// Line number for ordering within the bill.
    /// </summary>
    public int LineNumber { get; init; }

    /// <summary>
    /// Description of goods or services.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Quantity of items.
    /// </summary>
    public decimal Quantity { get; init; }

    /// <summary>
    /// Price per unit.
    /// </summary>
    public decimal UnitPrice { get; init; }

    /// <summary>
    /// Extended line amount (Quantity × UnitPrice).
    /// </summary>
    public decimal Amount { get; init; }

    /// <summary>
    /// Chart of account ID for posting.
    /// </summary>
    public DefaultIdType ChartOfAccountId { get; init; }

    /// <summary>
    /// Chart of account code.
    /// </summary>
    public string? ChartOfAccountCode { get; init; }

    /// <summary>
    /// Chart of account name.
    /// </summary>
    public string? ChartOfAccountName { get; init; }

    /// <summary>
    /// Tax code ID if applicable.
    /// </summary>
    public DefaultIdType? TaxCodeId { get; init; }

    /// <summary>
    /// Tax code name if applicable.
    /// </summary>
    public string? TaxCodeName { get; init; }

    /// <summary>
    /// Tax amount for this line.
    /// </summary>
    public decimal TaxAmount { get; init; }

    /// <summary>
    /// Project ID if applicable.
    /// </summary>
    public DefaultIdType? ProjectId { get; init; }

    /// <summary>
    /// Project name if applicable.
    /// </summary>
    public string? ProjectName { get; init; }

    /// <summary>
    /// Cost center ID if applicable.
    /// </summary>
    public DefaultIdType? CostCenterId { get; init; }

    /// <summary>
    /// Cost center name if applicable.
    /// </summary>
    public string? CostCenterName { get; init; }

    /// <summary>
    /// Optional notes for this line item.
    /// </summary>
    public string? Notes { get; init; }

    /// <summary>
    /// Date and time when the line item was created.
    /// </summary>
    public DateTimeOffset CreatedOn { get; init; }

    /// <summary>
    /// User who created the line item.
    /// </summary>
    public string? CreatedBy { get; init; }
}

