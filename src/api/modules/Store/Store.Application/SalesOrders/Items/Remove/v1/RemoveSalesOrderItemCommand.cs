namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Items.Remove.v1;

public sealed record RemoveSalesOrderItemCommand(
    DefaultIdType SalesOrderId,
    DefaultIdType GroceryItemId
) : IRequest;

