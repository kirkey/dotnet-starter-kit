namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Specs;

public class GetSalesOrderSpecification : Specification<SalesOrder>
{
    public GetSalesOrderSpecification(DefaultIdType id)
    {
        Query.Where(s => s.Id == id);
        Query.Include(s => s.Items);
        Query.Include(s => s.Customer);
        Query.Include(s => s.Warehouse);
    }
}

