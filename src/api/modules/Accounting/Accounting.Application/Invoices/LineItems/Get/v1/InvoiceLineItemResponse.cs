namespace Accounting.Application.Invoices.LineItems.Get.v1;

/// <summary>
/// Response model for invoice line item.
/// </summary>
public class InvoiceLineItemResponse
{
    /// <summary>
    /// Line item identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Parent invoice identifier.
    /// </summary>
    public DefaultIdType InvoiceId { get; set; }

    /// <summary>
    /// Line item description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Quantity of items or units.
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Price per unit.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Total line amount (Quantity × UnitPrice).
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Optional GL account identifier.
    /// </summary>
    public DefaultIdType? AccountId { get; set; }

    /// <summary>
    /// Date when the line item was created.
    /// </summary>
    public DateTimeOffset? CreatedOn { get; set; }

    /// <summary>
    /// Date when the line item was last modified.
    /// </summary>
    public DateTimeOffset? LastModifiedOn { get; set; }
}

