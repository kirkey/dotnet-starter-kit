namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Delete.v1;

public class DeleteInventoryTransactionCommand : IRequest<DeleteInventoryTransactionResponse>
{
    public DefaultIdType Id { get; set; }
}

public class DeleteInventoryTransactionResponse(DefaultIdType id, string transactionNumber)
{
    public DefaultIdType Id { get; } = id;
    public string TransactionNumber { get; } = transactionNumber;
}
