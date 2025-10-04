namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Search.v1;

public record GetPurchaseOrderListResponse(
    DefaultIdType Id,
    string Name,
    string? Description,
    string? Notes,
    string OrderNumber,
    DefaultIdType SupplierId,
    DateTime OrderDate,
    string Status,
    decimal TotalAmount);

