namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Get.v1;

public class GetCycleCountSpecs : Specification<CycleCount, CycleCountResponse>
{
    public GetCycleCountSpecs(DefaultIdType id)
    {
        Query
            .Include(c => c.Items)
            .Where(c => c.Id == id)
            .Select(c => new CycleCountResponse
            {
                Id = c.Id,
                Name = c.CountNumber,
                Description = null,
                CountNumber = c.CountNumber,
                WarehouseId = c.WarehouseId,
                WarehouseName = c.Warehouse.Name,
                WarehouseLocationId = c.WarehouseLocationId,
                WarehouseLocationName = c.WarehouseLocation != null ? c.WarehouseLocation.Name : null,
                CountDate = c.ScheduledDate,
                Status = c.Status,
                CountType = c.CountType,
                CountedBy = c.CounterName,
                StartDate = c.ActualStartDate,
                CompletedDate = c.CompletionDate,
                TotalItems = c.TotalItemsToCount,
                CountedItems = c.ItemsCountedCorrect,
                VarianceItems = c.ItemsWithDiscrepancies,
                AccuracyRate = c.TotalItemsToCount > 0 
                    ? Math.Round((decimal)(c.TotalItemsToCount - c.ItemsWithDiscrepancies) / c.TotalItemsToCount * 100, 2) 
                    : 0,
                Notes = c.Notes,
                Items = c.Items.Select(i => new CycleCountItemResponse(
                    i.Id,
                    i.ItemId,
                    i.SystemQuantity,
                    i.CountedQuantity,
                    i.VarianceQuantity,
                    i.CountDate,
                    i.CountedBy,
                    i.RequiresRecount
                )).ToList()
            });
    }
}
