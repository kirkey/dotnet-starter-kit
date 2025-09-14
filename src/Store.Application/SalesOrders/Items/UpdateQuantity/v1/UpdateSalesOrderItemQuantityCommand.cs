namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Items.UpdateQuantity.v1;

public sealed record UpdateSalesOrderItemQuantityCommand(
    DefaultIdType SalesOrderItemId,
    int Quantity
) : IRequest;

