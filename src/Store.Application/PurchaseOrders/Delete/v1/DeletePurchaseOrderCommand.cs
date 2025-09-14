namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Delete.v1;

public sealed record DeletePurchaseOrderCommand(DefaultIdType Id) : IRequest;

