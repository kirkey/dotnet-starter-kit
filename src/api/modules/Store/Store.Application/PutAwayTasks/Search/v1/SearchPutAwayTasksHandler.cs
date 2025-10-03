namespace FSH.Starter.WebApi.Store.Application.PutAwayTasks.Search.v1;

public sealed class SearchPutAwayTasksHandler(
    [FromKeyedServices("store:putawaytasks")] IRepository<PutAwayTask> repository)
    : IRequestHandler<SearchPutAwayTasksCommand, PagedList<PutAwayTaskResponse>>
{
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

        return new PagedList<PutAwayTaskResponse>(putAwayTaskResponses, totalCount, request.PageNumber, request.PageSize);
    }
}
