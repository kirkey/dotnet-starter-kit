namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Search.v1;

public record GetInventoryTransferListResponse(
    DefaultIdType Id,
    string TransferNumber,
    DefaultIdType FromWarehouseId,
    string FromWarehouseName,
    DefaultIdType ToWarehouseId,
    string ToWarehouseName,
    DateTime TransferDate,
    string Status,
    string TransferType,
    string Priority);
