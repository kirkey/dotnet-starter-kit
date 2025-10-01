namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Get.v1;

public sealed record GetPurchaseOrderQuery(DefaultIdType Id) : IRequest<GetPurchaseOrderResponse>;

