using FSH.Starter.WebApi.Store.Application.StockLevels.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.StockLevels.Specs;

/// <summary>
/// Specification to get a stock level by ID with response mapping.
/// </summary>
public sealed class GetStockLevelByIdSpec : Specification<StockLevel, StockLevelResponse>
{
    public GetStockLevelByIdSpec(DefaultIdType id)
    {
        Query.Where(s => s.Id == id);

        Query.Select(s => new StockLevelResponse(
            s.Id,
            s.ItemId,
            s.WarehouseId,
            s.WarehouseLocationId,
            s.BinId,
            s.LotNumberId,
            s.SerialNumberId,
            s.QuantityOnHand,
            s.QuantityAvailable,
            s.QuantityReserved,
            s.QuantityAllocated,
            s.LastCountDate,
            s.LastMovementDate,
            s.CreatedOn,
            s.CreatedBy));
    }
}
