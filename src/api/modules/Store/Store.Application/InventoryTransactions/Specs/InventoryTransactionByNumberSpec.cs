namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Specs;

public class InventoryTransactionByNumberSpec : Specification<InventoryTransaction>
{
    public InventoryTransactionByNumberSpec(string transactionNumber)
    {
        Query
            .Where(t => t.TransactionNumber == transactionNumber);
    }
}
