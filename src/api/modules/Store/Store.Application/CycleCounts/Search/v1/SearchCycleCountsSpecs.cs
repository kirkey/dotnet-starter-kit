using FSH.Starter.WebApi.Store.Application.CycleCounts.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Search.v1;

public class SearchCycleCountsSpecs : EntitiesByPaginationFilterSpec<CycleCount, CycleCountResponse>
{
    public SearchCycleCountsSpecs(SearchCycleCountsCommand command)
        : base(command) =>
        Query
            .OrderBy(c => c.ScheduledDate, !command.HasOrderBy())
            .Where(c => c.CountNumber!.Contains(command.CountNumber!), !string.IsNullOrEmpty(command.CountNumber))
            .Where(c => c.Status == command.Status, !string.IsNullOrEmpty(command.Status))
            .Where(c => c.WarehouseId == command.WarehouseId, command.WarehouseId.HasValue);
}

