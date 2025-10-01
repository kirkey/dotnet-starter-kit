namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Items.Add.v1;

public sealed record AddSalesOrderItemCommand(
    DefaultIdType SalesOrderId,
    DefaultIdType GroceryItemId,
    int Quantity,
    decimal UnitPrice,
    decimal? Discount = null
) : IRequest<AddSalesOrderItemResponse>;

