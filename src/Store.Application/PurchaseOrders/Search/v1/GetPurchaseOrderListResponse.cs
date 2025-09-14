namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Search.v1;

public record GetPurchaseOrderListResponse(
    DefaultIdType Id,
    string OrderNumber,
    DefaultIdType SupplierId,
    DateTime OrderDate,
    string Status,
    decimal TotalAmount);

