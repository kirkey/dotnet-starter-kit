namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Specs;

/// <summary>
/// Specification that filters purchase orders by SupplierId.
/// </summary>
public sealed class PurchaseOrdersBySupplierIdSpec : Specification<PurchaseOrder>
{
    public PurchaseOrdersBySupplierIdSpec(DefaultIdType supplierId)
    {
        Query.Where(p => p.SupplierId == supplierId);
    }
}

