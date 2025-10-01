namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Specs;

public sealed class InventoryTransactionsByPurchaseOrderIdSpec : Specification<InventoryTransaction>
{
    public InventoryTransactionsByPurchaseOrderIdSpec(DefaultIdType purchaseOrderId)
    {
        Query.Where(tx => tx.PurchaseOrderId == purchaseOrderId);
    }
}

