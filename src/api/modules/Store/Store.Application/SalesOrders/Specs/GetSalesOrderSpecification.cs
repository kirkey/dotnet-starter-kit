namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Specs;

public class GetSalesOrderSpecification : Specification<SalesOrder>
{
    public GetSalesOrderSpecification(DefaultIdType id)
    {
        Query.Where(s => s.Id == id);
        Query.Include(s => s.Items).ThenInclude(i => i.GroceryItem);
        // SalesOrder currently exposes CustomerId and WarehouseId but not navigation properties named
        // Customer or Warehouse; include related entities from Items (GroceryItem) which are present.
    }
}
