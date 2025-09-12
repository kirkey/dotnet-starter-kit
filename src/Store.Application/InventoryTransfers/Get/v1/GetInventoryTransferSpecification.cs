

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Get.v1;

public class GetInventoryTransferSpecification : Specification<InventoryTransfer>
{
    public GetInventoryTransferSpecification(DefaultIdType id)
    {
        Query.Where(it => it.Id == id);
        Query.Include(it => it.FromWarehouse);
        Query.Include(it => it.ToWarehouse);
    }
}
