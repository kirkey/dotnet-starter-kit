namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Specs;

public class PurchaseOrderByItemIdSpec : Specification<PurchaseOrder>
{
    public PurchaseOrderByItemIdSpec(DefaultIdType itemId)
    {
        Query.Where(po => po.Items.Any(i => i.Id == itemId));
        Query.Include(po => po.Items);
    }
}

