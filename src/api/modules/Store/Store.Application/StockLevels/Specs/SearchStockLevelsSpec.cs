using FSH.Starter.WebApi.Store.Application.StockLevels.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.StockLevels.Specs;

/// <summary>
/// Specification for searching stock levels with filters and pagination.
/// </summary>
public sealed class SearchStockLevelsSpec : EntitiesByPaginationFilterSpec<StockLevel, StockLevelResponse>
{
    public SearchStockLevelsSpec(SearchStockLevelsCommand request)
        : base(request)
    {
        Query
            .Include(s => s.Item)
            .Include(s => s.Warehouse)
            .Where(s => s.ItemId == request.ItemId, request.ItemId.HasValue)
            .Where(s => s.WarehouseId == request.WarehouseId, request.WarehouseId.HasValue)
            .Where(s => s.WarehouseLocationId == request.WarehouseLocationId, request.WarehouseLocationId.HasValue)
            .Where(s => s.BinId == request.BinId, request.BinId.HasValue)
            .Where(s => s.LotNumberId == request.LotNumberId, request.LotNumberId.HasValue)
            .Where(s => s.SerialNumberId == request.SerialNumberId, request.SerialNumberId.HasValue)
            .Where(s => s.QuantityOnHand >= request.MinQuantityOnHand, request.MinQuantityOnHand.HasValue)
            .Where(s => s.QuantityOnHand <= request.MaxQuantityOnHand, request.MaxQuantityOnHand.HasValue)
            .Where(s => s.QuantityAvailable >= request.MinQuantityAvailable, request.MinQuantityAvailable.HasValue)
            .Where(s => s.QuantityReserved > 0, request.HasReservedQuantity == true)
            .Where(s => s.QuantityReserved == 0, request.HasReservedQuantity == false)
            .Where(s => s.QuantityAllocated > 0, request.HasAllocatedQuantity == true)
            .Where(s => s.QuantityAllocated == 0, request.HasAllocatedQuantity == false);
    }
}
