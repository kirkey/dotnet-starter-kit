namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Update.v1;

public record UpdateInventoryTransferCommand(
    DefaultIdType Id,
    [property: DefaultValue("Transfer to Main Warehouse")] string Name,
    [property: DefaultValue("Transfer items between warehouses")] string? Description,
    [property: DefaultValue("TRF001")] string TransferNumber,
    DefaultIdType FromWarehouseId,
    DefaultIdType ToWarehouseId,
    [property: DefaultValue("2024-01-01")] DateTime TransferDate,
    [property: DefaultValue("Pending")] string Status,
    [property: DefaultValue(null)] string? Notes,
    [property: DefaultValue("Inventory Rebalancing")] string Reason) : IRequest<UpdateInventoryTransferResponse>;
