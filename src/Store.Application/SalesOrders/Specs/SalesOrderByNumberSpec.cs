namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Specs;

public class SalesOrderByNumberSpec : Specification<SalesOrder>, ISingleResultSpecification<SalesOrder>
{
    public SalesOrderByNumberSpec(string orderNumber, DefaultIdType? excludeId = null)
    {
        var normalized = orderNumber.Trim();
        Query
            .Where(so => so.OrderNumber == normalized)
            .Where(so => so.Id != excludeId, excludeId.HasValue);
    }
}

