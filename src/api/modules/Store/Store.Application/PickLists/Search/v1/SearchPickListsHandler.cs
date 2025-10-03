namespace FSH.Starter.WebApi.Store.Application.PickLists.Search.v1;

public sealed class SearchPickListsHandler([FromKeyedServices("store:picklists")] IReadRepository<PickList> repository)
    : IRequestHandler<SearchPickListsCommand, PagedList<PickListResponse>>
{
    public async Task<PagedList<PickListResponse>> Handle(SearchPickListsCommand request, CancellationToken cancellationToken)
    {
        var spec = new SearchPickListsSpec(request);
        var pickLists = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<PickListDto>(
            pickLists,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}
