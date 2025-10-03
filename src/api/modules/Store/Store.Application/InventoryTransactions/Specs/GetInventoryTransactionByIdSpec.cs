namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Specs;

public class GetInventoryTransactionByIdSpec : Specification<InventoryTransaction>
{
    public GetInventoryTransactionByIdSpec(DefaultIdType id)
    {
        Query
            .Where(t => t.Id == id);
    }
}
