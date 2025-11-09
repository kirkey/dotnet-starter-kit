using FSH.Starter.WebApi.Store.Application.WarehouseLocations.Specs;

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
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Retrieved {Count} warehouse locations", totalCount);
        
        return new PagedList<GetWarehouseLocationListResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
