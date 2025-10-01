namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Get.v1;

public sealed record GetSalesOrderResponse(
    DefaultIdType Id,
    string OrderNumber,
    DefaultIdType CustomerId,
    DateTime OrderDate,
    DateTime? DeliveryDate,
    string Status,
    string OrderType,
    decimal SubTotal,
    decimal TaxAmount,
    decimal DiscountAmount,
    decimal ShippingAmount,
    decimal TotalAmount,
    string PaymentStatus,
    string PaymentMethod,
    string? DeliveryAddress,
    bool IsUrgent,
    string? SalesPersonId,
    DefaultIdType? WarehouseId,
    DateTimeOffset CreatedOn,
    DateTimeOffset LastModifiedOn);
