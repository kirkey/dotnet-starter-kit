namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Create.v1;

public sealed record CreateSalesOrderCommand(
    DefaultIdType CustomerId,
    decimal Total)
    : IRequest<CreateSalesOrderResponse>;
