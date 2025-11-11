namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Get.v1;

public sealed record PurchaseOrderResponse(
    DefaultIdType Id,
    string Name,
    string? Description,
    string? Notes,
    string OrderNumber,
    DefaultIdType SupplierId,
    string? SupplierName,
    DateTime OrderDate,
    DateTime? ExpectedDeliveryDate,
    DateTime? ActualDeliveryDate,
    string Status,
    decimal TotalAmount,
    decimal TaxAmount,
    decimal DiscountAmount,
    decimal NetAmount,
    string? DeliveryAddress,
    string? ContactPerson,
    string? ContactPhone,
    bool IsUrgent,
    DateTimeOffset CreatedOn,
    DateTimeOffset LastModifiedOn);

