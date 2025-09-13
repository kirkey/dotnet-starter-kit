namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Get.v1;

public sealed record GetSalesOrderQuery(DefaultIdType Id) : IRequest<GetSalesOrderResponse>;

