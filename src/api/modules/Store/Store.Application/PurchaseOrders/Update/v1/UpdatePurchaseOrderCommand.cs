namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Update.v1;

/// <summary>
/// Command to update an existing purchase order.
/// </summary>
public record UpdatePurchaseOrderCommand : IRequest<UpdatePurchaseOrderResponse>
{
    /// <summary>
    /// Gets or sets the purchase order identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Gets or sets the order number.
    /// </summary>
    [DefaultValue("PO-2025-001")]
    public string OrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the supplier identifier.
    /// </summary>
    public DefaultIdType? SupplierId { get; set; }

    /// <summary>
    /// Gets or sets the order date.
    /// </summary>
    [DefaultValue("2025-10-04")]
    public DateTime? OrderDate { get; set; }

    /// <summary>
    /// Gets or sets the expected delivery date.
    /// </summary>
    [DefaultValue(null)]
    public DateTime? ExpectedDeliveryDate { get; set; }

    /// <summary>
    /// Gets or sets the actual delivery date.
    /// </summary>
    [DefaultValue(null)]
    public DateTime? ActualDeliveryDate { get; set; }

    /// <summary>
    /// Gets or sets the status (Draft, Submitted, Approved, Sent, Received, Cancelled).
    /// </summary>
    [DefaultValue("Draft")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total amount.
    /// </summary>
    [DefaultValue(0)]
    public decimal? TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the tax amount.
    /// </summary>
    [DefaultValue(0)]
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// Gets or sets the discount amount.
    /// </summary>
    [DefaultValue(0)]
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// Gets or sets the shipping cost.
    /// </summary>
    [DefaultValue(0)]
    public decimal? ShippingCost { get; set; }

    /// <summary>
    /// Gets or sets the net amount.
    /// </summary>
    [DefaultValue(0)]
    public decimal? NetAmount { get; set; }

    /// <summary>
    /// Gets or sets the delivery address.
    /// </summary>
    [DefaultValue(null)]
    public string? DeliveryAddress { get; set; }

    /// <summary>
    /// Gets or sets the contact person.
    /// </summary>
    [DefaultValue(null)]
    public string? ContactPerson { get; set; }

    /// <summary>
    /// Gets or sets the contact phone.
    /// </summary>
    [DefaultValue(null)]
    public string? ContactPhone { get; set; }

    /// <summary>
    /// Gets or sets whether the order is urgent.
    /// </summary>
    [DefaultValue(false)]
    public bool? IsUrgent { get; set; }

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    [DefaultValue(null)]
    public string? Notes { get; set; }
}

