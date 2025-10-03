namespace FSH.Starter.WebApi.Store.Application.PickLists.Search.v1;

public sealed class SearchPickListsHandler([FromKeyedServices("store:picklists")] IReadRepository<PickList> repository)
    : IRequestHandler<SearchPickListsCommand, PagedList<PickListResponse>>
{
    public async Task<PagedList<PickListResponse>> Handle(SearchPickListsCommand request, CancellationToken cancellationToken)
    {
        var spec = new SearchPickListsSpec(request);
        var pickLists = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var pickListResponses = pickLists.Select(p => new PickListResponse
        {
            Id = p.Id,
            PickListNumber = p.PickListNumber,
            WarehouseId = p.WarehouseId,
            Status = p.Status,
            PickingType = p.PickingType,
            Priority = p.Priority,
            AssignedTo = p.AssignedTo,
            StartDate = p.StartDate,
            CompletedDate = p.CompletedDate,
            ReferenceNumber = p.ReferenceNumber,
            TotalLines = p.TotalLines, // Use property directly from domain model
            CompletedLines = p.CompletedLines // Use property directly from domain model
        }).ToList();

        return new PagedList<PickListResponse>(
            pickListResponses,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}
