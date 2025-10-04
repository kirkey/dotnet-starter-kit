namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Update.v1;

/// <summary>
/// Command to update an existing Purchase Order entity.
/// Includes order details, supplier reference, delivery information, and status.
/// </summary>
public sealed record UpdatePurchaseOrderCommand(
    DefaultIdType Id,
    [property: DefaultValue("PO-0001")] string? OrderNumber,
    DefaultIdType? SupplierId,
    DateTime? OrderDate,
    DateTime? ExpectedDeliveryDate = null,
    DateTime? ActualDeliveryDate = null,
    [property: DefaultValue("Draft")] string? Status = "Draft",
    [property: DefaultValue(null)] string? Notes = null,
    [property: DefaultValue(null)] string? DeliveryAddress = null,
    [property: DefaultValue(null)] string? ContactPerson = null,
    [property: DefaultValue(null)] string? ContactPhone = null,
    [property: DefaultValue(false)] bool? IsUrgent = false,
    decimal? TaxAmount = null,
    decimal? DiscountAmount = null
) : IRequest<UpdatePurchaseOrderResponse>;

