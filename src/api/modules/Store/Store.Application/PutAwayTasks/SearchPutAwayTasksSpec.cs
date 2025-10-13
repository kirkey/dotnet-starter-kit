using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks;

/// <summary>
/// Specification for searching put away tasks with various filters and pagination support.
/// </summary>
public sealed class SearchPutAwayTasksSpec : EntitiesByPaginationFilterSpec<PutAwayTask>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchPutAwayTasksSpec"/> class.
    /// </summary>
    /// <param name="request">The search put away tasks command containing filter criteria and pagination parameters.</param>
    public SearchPutAwayTasksSpec(SearchPutAwayTasksCommand request)
        : base(request)
    {
        Query
            .Where(p => p.TaskNumber.Contains(request.TaskNumber!), !string.IsNullOrWhiteSpace(request.TaskNumber))
            .Where(p => p.WarehouseId == request.WarehouseId, request.WarehouseId.HasValue)
            .Where(p => p.GoodsReceiptId == request.GoodsReceiptId, request.GoodsReceiptId.HasValue)
            .Where(p => p.Status == request.Status, !string.IsNullOrWhiteSpace(request.Status))
            .Where(p => p.Priority >= request.MinPriority!.Value, request.MinPriority.HasValue)
            .Where(p => p.Priority <= request.MaxPriority!.Value, request.MaxPriority.HasValue)
            .Where(p => p.AssignedTo!.Contains(request.AssignedTo!), !string.IsNullOrWhiteSpace(request.AssignedTo))
            .Where(p => p.PutAwayStrategy!.Contains(request.PutAwayStrategy!), !string.IsNullOrWhiteSpace(request.PutAwayStrategy))
            .Where(p => p.StartDate >= request.StartDateFrom, request.StartDateFrom.HasValue)
            .Where(p => p.StartDate <= request.StartDateTo, request.StartDateTo.HasValue)
            .Where(p => p.CompletedDate >= request.CompletedDateFrom, request.CompletedDateFrom.HasValue)
            .Where(p => p.CompletedDate <= request.CompletedDateTo, request.CompletedDateTo.HasValue)
            .OrderBy(p => p.TaskNumber, !request.HasOrderBy());
    }
}
