namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Get.v1;

public sealed record GetSalesOrderResponse(
    DefaultIdType Id,
    DefaultIdType? CustomerId,
    decimal Total
);

