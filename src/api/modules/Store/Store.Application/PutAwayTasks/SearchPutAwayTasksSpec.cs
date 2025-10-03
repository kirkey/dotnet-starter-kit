using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks;

public sealed class SearchPutAwayTasksSpec : Specification<PutAwayTask>
{
    public SearchPutAwayTasksSpec(SearchPutAwayTasksCommand request)
    {
        Query
            .Where(p => p.TaskNumber.Contains(request.TaskNumber!), !string.IsNullOrWhiteSpace(request.TaskNumber))
            .Where(p => p.WarehouseId == request.WarehouseId, request.WarehouseId.HasValue)
            .Where(p => p.GoodsReceiptId == request.GoodsReceiptId, request.GoodsReceiptId.HasValue)
            .Where(p => p.Status == request.Status, !string.IsNullOrWhiteSpace(request.Status))
            .Where(p => p.Priority >= request.MinPriority, request.MinPriority.HasValue)
            .Where(p => p.Priority <= request.MaxPriority, request.MaxPriority.HasValue)
            .Where(p => p.AssignedTo!.Contains(request.AssignedTo!), !string.IsNullOrWhiteSpace(request.AssignedTo))
            .Where(p => p.PutAwayStrategy!.Contains(request.PutAwayStrategy!), !string.IsNullOrWhiteSpace(request.PutAwayStrategy))
            .Where(p => p.StartDate >= request.StartDateFrom, request.StartDateFrom.HasValue)
            .Where(p => p.StartDate <= request.StartDateTo, request.StartDateTo.HasValue)
            .Where(p => p.CompletedDate >= request.CompletedDateFrom, request.CompletedDateFrom.HasValue)
            .Where(p => p.CompletedDate <= request.CompletedDateTo, request.CompletedDateTo.HasValue)
            .OrderBy(p => p.TaskNumber)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);
    }
}
