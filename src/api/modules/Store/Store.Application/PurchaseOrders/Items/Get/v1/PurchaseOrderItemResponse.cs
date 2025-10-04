namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.Get.v1;

public sealed record PurchaseOrderItemResponse(
    DefaultIdType Id,
    DefaultIdType PurchaseOrderId,
    DefaultIdType ItemId,
    string ItemName,
    string ItemSku,
    int Quantity,
    decimal UnitPrice,
    decimal DiscountAmount,
    decimal TotalPrice,
    int ReceivedQuantity,
    string? Notes);
