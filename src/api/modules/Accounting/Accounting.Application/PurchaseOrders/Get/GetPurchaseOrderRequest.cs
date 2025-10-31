using Accounting.Application.PurchaseOrders.Responses;

namespace Accounting.Application.PurchaseOrders.Get;

/// <summary>
/// Request to get a purchase order by ID.
/// </summary>
public record GetPurchaseOrderRequest(DefaultIdType Id) : IRequest<PurchaseOrderResponse>;

