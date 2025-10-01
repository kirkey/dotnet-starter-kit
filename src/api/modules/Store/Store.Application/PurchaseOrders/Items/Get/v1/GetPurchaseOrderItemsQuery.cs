namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.Get.v1;

public sealed record GetPurchaseOrderItemsQuery(DefaultIdType PurchaseOrderId) : IRequest<List<PurchaseOrderItemResponse>>;
