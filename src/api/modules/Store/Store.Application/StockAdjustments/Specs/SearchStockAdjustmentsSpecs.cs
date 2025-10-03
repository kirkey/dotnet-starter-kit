using FSH.Starter.WebApi.Store.Application.StockAdjustments.Get.v1;
using FSH.Starter.WebApi.Store.Application.StockAdjustments.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Specs;

public class SearchStockAdjustmentsSpecs : EntitiesByPaginationFilterSpec<StockAdjustment, StockAdjustmentResponse>
{
    public SearchStockAdjustmentsSpecs(SearchStockAdjustmentsCommand command)
        : base(command)
    {
        Query
            .Where(x => x.ItemId == command.ItemId, command.ItemId.HasValue)
            .Where(x => x.WarehouseLocationId == command.WarehouseLocationId, command.WarehouseLocationId.HasValue)
            .Where(x => x.AdjustmentType == command.AdjustmentType, !string.IsNullOrWhiteSpace(command.AdjustmentType))
            .Where(x => x.Reason != null && command.Reason != null && x.Reason.Contains(command.Reason), !string.IsNullOrWhiteSpace(command.Reason))
            .Where(x => x.AdjustmentDate >= command.DateFrom, command.DateFrom.HasValue)
            .Where(x => x.AdjustmentDate <= command.DateTo, command.DateTo.HasValue)
            .OrderByDescending(x => x.AdjustmentDate, !command.HasOrderBy());
    }
}
