using Accounting.Application.PurchaseOrders.Responses;

namespace Accounting.Application.PurchaseOrders.Search.v1;

/// <summary>
/// Request to search for purchase orders with optional filters.
/// </summary>
public record SearchPurchaseOrdersRequest(
    string? OrderNumber = null,
    DefaultIdType? VendorId = null,
    string? Status = null,
    bool? IsFullyReceived = null) : IRequest<List<PurchaseOrderResponse>>;
