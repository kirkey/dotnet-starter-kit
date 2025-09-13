using FSH.Framework.Core.Paging;

namespace FSH.Starter.WebApi.Store.Application.WarehouseLocations.Search.v1;

public sealed class SearchWarehouseLocationsHandler(
    ILogger<SearchWarehouseLocationsHandler> logger,
    [FromKeyedServices("store:warehouse-locations")] IRepository<WarehouseLocation> repository)
    : IRequestHandler<SearchWarehouseLocationsCommand, PagedList<GetWarehouseLocationListResponse>>
{
    public async Task<PagedList<GetWarehouseLocationListResponse>> Handle(SearchWarehouseLocationsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var spec = new GetWarehouseLocationListSpecification(request);
        var paged = await repository.PaginatedListAsync(spec, new PaginationFilter { PageNumber = request.PageNumber, PageSize = request.PageSize }, cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Retrieved {Count} warehouse locations", paged.TotalCount);
        
        return paged;
    }
}
