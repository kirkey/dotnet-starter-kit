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

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<PutAwayTaskResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
