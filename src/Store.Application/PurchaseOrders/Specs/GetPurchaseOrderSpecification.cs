namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Specs;

public class GetPurchaseOrderSpecification : Specification<PurchaseOrder>
{
    public GetPurchaseOrderSpecification(DefaultIdType id)
    {
        Query.Where(po => po.Id == id);
        Query.Include(po => po.Items);
        Query.Include(po => po.Supplier);
    }
}

