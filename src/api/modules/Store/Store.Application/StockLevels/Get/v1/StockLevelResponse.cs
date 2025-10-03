namespace FSH.Starter.WebApi.Store.Application.StockLevels.Get.v1;

/// <summary>
/// Response containing stock level details.
/// </summary>
public sealed record StockLevelResponse(
    DefaultIdType Id,
    DefaultIdType ItemId,
    DefaultIdType WarehouseId,
    DefaultIdType? WarehouseLocationId,
    DefaultIdType? BinId,
    DefaultIdType? LotNumberId,
    DefaultIdType? SerialNumberId,
    int QuantityOnHand,
    int QuantityAvailable,
    int QuantityReserved,
    int QuantityAllocated,
    DateTime? LastCountDate,
    DateTime? LastMovementDate,
    DateTimeOffset CreatedOn,
    DefaultIdType CreatedBy
);
