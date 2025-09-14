namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Specs;

public class SalesOrderByItemIdSpec : Specification<SalesOrder>
{
    public SalesOrderByItemIdSpec(DefaultIdType itemId)
    {
        Query.Where(so => so.Items.Any(i => i.Id == itemId));
        Query.Include(so => so.Items);
    }
}

