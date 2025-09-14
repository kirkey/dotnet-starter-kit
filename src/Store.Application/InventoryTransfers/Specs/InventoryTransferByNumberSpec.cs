namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Specs;

public class InventoryTransferByNumberSpec : Specification<InventoryTransfer>
{
    public InventoryTransferByNumberSpec(string transferNumber)
    {
        Query.Where(t => t.TransferNumber == transferNumber);
    }
}

