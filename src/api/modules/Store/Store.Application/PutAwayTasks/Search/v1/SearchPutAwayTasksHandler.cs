namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Search.v1;

/// <summary>
/// Handler for searching and filtering put-away tasks with pagination.
/// </summary>
public sealed class SearchPutAwayTasksHandler(
    [FromKeyedServices("store:putawaytasks")] IRepository<PutAwayTask> repository)
    : IRequestHandler<SearchPutAwayTasksCommand, PagedList<PutAwayTaskResponse>>
{
    /// <summary>
    /// Handles the search put-away tasks command.
    /// </summary>
    /// <param name="request">The search command with filter criteria and pagination settings.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paged list of put-away task responses.</returns>
    public async Task<PagedList<PutAwayTaskResponse>> Handle(SearchPutAwayTasksCommand request, CancellationToken cancellationToken)
    {
        var spec = new SearchPutAwayTasksSpec(request);

        var putAwayTasks = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var putAwayTaskResponses = putAwayTasks.Select(p => new PutAwayTaskResponse
        {
            Id = p.Id,
            TaskNumber = p.TaskNumber,
            WarehouseId = p.WarehouseId,
            WarehouseName = p.Warehouse.Name,
            GoodsReceiptId = p.GoodsReceiptId,
            Status = p.Status,
            Priority = p.Priority,
            AssignedTo = p.AssignedTo,
            StartDate = p.StartDate,
            CompletedDate = p.CompletedDate,
            PutAwayStrategy = p.PutAwayStrategy,
            Notes = p.Notes,
            TotalLines = p.TotalLines,
            CompletedLines = p.CompletedLines
        }).ToList();

        return new PagedList<PutAwayTaskResponse>(
            putAwayTaskResponses,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}
