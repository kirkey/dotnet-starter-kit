namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Delete.v1;

public sealed record DeleteSalesOrderCommand(
    DefaultIdType Id) : IRequest;

