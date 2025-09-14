namespace FSH.Starter.WebApi.Store.Application.PriceLists.Search.v1;

public sealed class GetPriceListListHandler(
    ILogger<GetPriceListListHandler> logger,
    [FromKeyedServices("store:price-lists")] IRepository<PriceList> repository)
    : IRequestHandler<SearchPriceListsCommand, PagedList<GetPriceListListResponse>>
{
    public async Task<PagedList<GetPriceListListResponse>> Handle(SearchPriceListsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new Specs.GetPriceListListSpecification(request);
        var paged = await repository.PaginatedListAsync(spec, new PaginationFilter { PageNumber = request.PageNumber, PageSize = request.PageSize }, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Search complete: retrieved {Count} price lists", paged.TotalCount);
        return paged;
    }
}

