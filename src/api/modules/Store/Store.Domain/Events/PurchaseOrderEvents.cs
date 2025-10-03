using Store.Domain.Entities;

namespace Store.Domain.Events;

/// <summary>
/// Raised when a purchase order is created.
/// </summary>
public record PurchaseOrderCreated : DomainEvent
{
    /// <summary>The purchase order instance that was created.</summary>
    public PurchaseOrder PurchaseOrder { get; init; } = default!;
}

/// <summary>
/// Raised when the status of a purchase order changes.
/// </summary>
public record PurchaseOrderStatusChanged : DomainEvent
{
    /// <summary>The purchase order whose status changed.</summary>
    public PurchaseOrder PurchaseOrder { get; init; } = default!;
    /// <summary>The previous status value.</summary>
    public string PreviousStatus { get; init; } = default!;
    /// <summary>The new status value.</summary>
    public string NewStatus { get; init; } = default!;
}

/// <summary>
/// Raised when a draft purchase order is submitted for approval.
/// </summary>
public record PurchaseOrderSubmitted : DomainEvent
{
    /// <summary>The submitted purchase order.</summary>
    public PurchaseOrder PurchaseOrder { get; init; } = default!;
}

/// <summary>
/// Raised when a purchase order is approved.
/// </summary>
public record PurchaseOrderApproved : DomainEvent
{
    /// <summary>The approved purchase order.</summary>
    public PurchaseOrder PurchaseOrder { get; init; } = default!;
}

/// <summary>
/// Raised when a purchase order is cancelled.
/// </summary>
public record PurchaseOrderCancelled : DomainEvent
{
    /// <summary>The cancelled purchase order.</summary>
    public PurchaseOrder PurchaseOrder { get; init; } = default!;
}

/// <summary>
/// Raised when a purchase order is sent to the supplier.
/// </summary>
public record PurchaseOrderSentEvent : DomainEvent
{
    /// <summary>The purchase order that was dispatched.</summary>
    public PurchaseOrder PurchaseOrder { get; init; } = default!;
}

/// <summary>
/// Raised when a new line item is added to a purchase order.
/// </summary>
public record PurchaseOrderItemAdded : DomainEvent
{
    /// <summary>The affected purchase order.</summary>
    public PurchaseOrder PurchaseOrder { get; init; } = default!;
    /// <summary>The grocery item identifier of the added line.</summary>
    public DefaultIdType ItemId { get; init; }
}

/// <summary>
/// Event raised when an item is removed from a purchase order.
/// </summary>
public sealed record PurchaseOrderItemRemoved : DomainEvent
{
    public PurchaseOrder PurchaseOrder { get; init; } = default!;
    public DefaultIdType ItemId { get; init; }
    /// <summary>The grocery item identifier of the removed line.</summary>
    public DefaultIdType GroceryItemId { get; init; }
}

/// <summary>
/// Raised when a purchase order is marked as delivered/received.
/// </summary>
public record PurchaseOrderDelivered : DomainEvent
{
    /// <summary>The purchase order that was received.</summary>
    public PurchaseOrder PurchaseOrder { get; init; } = default!;
}

/// <summary>
/// Raised when a header-level discount is applied to a purchase order.
/// </summary>
public record PurchaseOrderDiscountApplied : DomainEvent
{
    /// <summary>The affected purchase order.</summary>
    public PurchaseOrder PurchaseOrder { get; init; } = default!;
    /// <summary>The new discount amount applied to the order.</summary>
    public decimal DiscountAmount { get; init; }
}

/// <summary>
/// Raised when a purchase order line item is created.
/// </summary>
public record PurchaseOrderItemCreated : DomainEvent
{
    /// <summary>The created line item.</summary>
    public PurchaseOrderItem PurchaseOrderItem { get; init; } = default!;
}

/// <summary>
/// Raised when the quantity of a purchase order line item changes.
/// </summary>
public record PurchaseOrderItemQuantityUpdated : DomainEvent
{
    /// <summary>The updated line item.</summary>
    public PurchaseOrderItem PurchaseOrderItem { get; init; } = default!;
}

/// <summary>
/// Raised when the price or discount of a purchase order line item changes.
/// </summary>
public record PurchaseOrderItemPriceUpdated : DomainEvent
{
    /// <summary>The updated line item.</summary>
    public PurchaseOrderItem PurchaseOrderItem { get; init; } = default!;
}

/// <summary>
/// Raised when a received quantity update is recorded for a line item.
/// </summary>
public record PurchaseOrderItemReceived : DomainEvent
{
    /// <summary>The affected line item.</summary>
    public PurchaseOrderItem PurchaseOrderItem { get; init; } = default!;
    /// <summary>The previous received quantity.</summary>
    public int PreviousReceivedQuantity { get; init; }
    /// <summary>The new received quantity value.</summary>
    public int NewReceivedQuantity { get; init; }
}

/// <summary>
/// Raised when a purchase order header is updated (fields changed).
/// </summary>
public record PurchaseOrderUpdated : DomainEvent
{
    /// <summary>The updated purchase order.</summary>
    public PurchaseOrder PurchaseOrder { get; init; } = default!;
}
