using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Get.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders;

/// <summary>
/// Extension methods for adapting PurchaseOrder domain entities to response DTOs.
/// Provides clean mapping between domain objects and API responses following DRY principles.
/// </summary>
public static class PurchaseOrderAdaptExtensions
{
    /// <summary>
    /// Adapts a PurchaseOrder domain entity to PurchaseOrderResponse DTO.
    /// </summary>
    /// <param name="purchaseOrder">The purchase order domain entity</param>
    /// <returns>Mapped PurchaseOrderResponse DTO</returns>
    public static PurchaseOrderResponse Adapt(this PurchaseOrder purchaseOrder)
    {
        return new PurchaseOrderResponse(
            purchaseOrder.Id,
            purchaseOrder.OrderNumber,
            purchaseOrder.SupplierId,
            purchaseOrder.OrderDate,
            purchaseOrder.ExpectedDeliveryDate,
            purchaseOrder.ActualDeliveryDate,
            purchaseOrder.Status,
            purchaseOrder.TotalAmount,
            purchaseOrder.TaxAmount,
            purchaseOrder.DiscountAmount,
            purchaseOrder.NetAmount,
            purchaseOrder.DeliveryAddress,
            purchaseOrder.ContactPerson,
            purchaseOrder.ContactPhone,
            purchaseOrder.IsUrgent,
            purchaseOrder.CreatedOn,
            purchaseOrder.LastModifiedOn);
    }

    /// <summary>
    /// Adapts a PurchaseOrder domain entity to GetPurchaseOrderListResponse DTO for search results.
    /// </summary>
    /// <param name="purchaseOrder">The purchase order domain entity</param>
    /// <returns>Mapped GetPurchaseOrderListResponse DTO</returns>
    public static GetPurchaseOrderListResponse AdaptToListResponse(this PurchaseOrder purchaseOrder)
    {
        return new GetPurchaseOrderListResponse(
            purchaseOrder.Id,
            purchaseOrder.OrderNumber,
            purchaseOrder.SupplierId,
            purchaseOrder.OrderDate,
            purchaseOrder.Status,
            purchaseOrder.TotalAmount);
    }
}
