namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Specs;

public class PurchaseOrderByNumberSpec : Specification<PurchaseOrder>
{
    public PurchaseOrderByNumberSpec(string orderNumber)
    {
        Query.Where(po => po.OrderNumber == orderNumber);
    }
}

