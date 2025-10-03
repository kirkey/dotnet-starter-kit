namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Get.v1;

public class GetInventoryTransactionCommand : IRequest<InventoryTransactionResponse>
{
    public DefaultIdType Id { get; set; }
}
