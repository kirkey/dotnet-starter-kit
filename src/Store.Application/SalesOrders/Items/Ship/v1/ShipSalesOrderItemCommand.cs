namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Items.Ship.v1;

public sealed record ShipSalesOrderItemCommand(
    DefaultIdType SalesOrderItemId,
    int ShippedQuantity
) : IRequest;

