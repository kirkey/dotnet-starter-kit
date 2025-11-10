using FSH.Starter.WebApi.Store.Application.CycleCounts.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Search.v1;

public class SearchCycleCountsSpecs : Specification<CycleCount, CycleCountResponse>
{
    public SearchCycleCountsSpecs(SearchCycleCountsRequest request)
    {
        Query
            .Include(c => c.Warehouse)
            .Include(c => c.WarehouseLocation)
            .OrderBy(c => c.ScheduledDate, !request.HasOrderBy())
            .Where(c => c.CountNumber.Contains(request.CountNumber ?? string.Empty), !string.IsNullOrEmpty(request.CountNumber))
            .Where(c => c.Status == request.Status, !string.IsNullOrEmpty(request.Status))
            .Where(c => c.WarehouseId == request.WarehouseId, request.WarehouseId.HasValue)
            .Where(c => c.ScheduledDate >= request.CountDateFrom, request.CountDateFrom.HasValue)
            .Where(c => c.ScheduledDate <= request.CountDateTo, request.CountDateTo.HasValue)
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
                Items = null
            });
        
        if (!string.IsNullOrEmpty(request.Keyword))
        {
            Query.Search(x => x.CountNumber, $"%{request.Keyword}%");
        }
    }
}

