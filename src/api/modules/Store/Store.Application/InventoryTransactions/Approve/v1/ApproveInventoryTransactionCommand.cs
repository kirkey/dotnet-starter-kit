using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Approve.v1;

public class ApproveInventoryTransactionCommand : IRequest<ApproveInventoryTransactionResponse>
{
    public DefaultIdType Id { get; set; }
    public string ApprovedBy { get; set; } = default!;
}

public class ApproveInventoryTransactionResponse(InventoryTransactionResponse transaction)
{
    public InventoryTransactionResponse Transaction { get; } = transaction;
}
