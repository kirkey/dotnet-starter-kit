namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Create.v1;

public sealed record CreatePurchaseOrderCommand(
    [property: DefaultValue("PO-0001")] string OrderNumber,
    DefaultIdType SupplierId,
    DateTime OrderDate,
    DateTime? ExpectedDeliveryDate = null,
    [property: DefaultValue("Draft")] string Status = "Draft",
    [property: DefaultValue(null)] string? Notes = null,
    [property: DefaultValue(null)] string? DeliveryAddress = null,
    [property: DefaultValue(null)] string? ContactPerson = null,
    [property: DefaultValue(null)] string? ContactPhone = null,
    [property: DefaultValue(false)] bool IsUrgent = false
) : IRequest<CreatePurchaseOrderResponse>;

