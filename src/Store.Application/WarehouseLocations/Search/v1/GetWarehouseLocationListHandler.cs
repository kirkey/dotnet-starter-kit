

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
        var warehouseLocations = await repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Retrieved {Count} warehouse locations", warehouseLocations.Data.Count);
        
        return warehouseLocations.Select(wl => new GetWarehouseLocationListResponse(
            wl.Id,
            wl.Name!,
            wl.Code,
            wl.Aisle,
            wl.Section,
            wl.Shelf,
            wl.Bin,
            wl.WarehouseId,
            wl.Warehouse.Name!,
            wl.LocationType,
            wl.Capacity,
            wl.UsedCapacity,
            wl.CapacityUnit,
            wl.IsActive,
            wl.RequiresTemperatureControl));
    }
}
