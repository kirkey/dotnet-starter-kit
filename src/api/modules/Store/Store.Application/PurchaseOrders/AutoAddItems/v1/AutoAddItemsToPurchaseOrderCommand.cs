namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.AutoAddItems.v1;

/// <summary>
/// Command to automatically add items needing reorder to a purchase order.
/// </summary>
public record AutoAddItemsToPurchaseOrderCommand : IRequest<AutoAddItemsToPurchaseOrderResponse>
{
    /// <summary>
    /// Purchase order ID to add items to.
    /// </summary>
    public DefaultIdType PurchaseOrderId { get; init; }

    /// <summary>
    /// Optional warehouse ID to check stock levels at specific location.
    /// If null, checks total stock across all warehouses.
    /// </summary>
    public DefaultIdType? WarehouseId { get; init; }

    /// <summary>
    /// Whether to use suggested quantities (true) or reorder quantities (false).
    /// Default: true (use smart calculation).
    /// </summary>
    public bool UseSuggestedQuantities { get; init; } = true;
}

