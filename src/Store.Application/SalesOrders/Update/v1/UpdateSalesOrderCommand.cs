namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Update.v1;

public sealed record UpdateSalesOrderCommand(
    DefaultIdType Id,
    DefaultIdType? CustomerId,
    decimal Total)
    : IRequest<UpdateSalesOrderResponse>;

