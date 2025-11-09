namespace FSH.Starter.WebApi.Store.Application.PickLists.Search.v1;

/// <summary>
/// Handler for searching and filtering pick lists with pagination.
/// </summary>
public sealed class SearchPickListsHandler([FromKeyedServices("store:picklists")] IReadRepository<PickList> repository)
    : IRequestHandler<SearchPickListsCommand, PagedList<PickListResponse>>
{
    /// <summary>
    /// Handles the search pick lists command.
    /// </summary>
    /// <param name="request">The search command with filter criteria and pagination settings.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paged list of pick list responses.</returns>
    public async Task<PagedList<PickListResponse>> Handle(SearchPickListsCommand request, CancellationToken cancellationToken)
    {
        var spec = new SearchPickListsSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<PickListResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
