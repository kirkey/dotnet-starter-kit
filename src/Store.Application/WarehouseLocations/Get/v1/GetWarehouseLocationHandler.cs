using FSH.Starter.WebApi.Store.Application.WarehouseLocations.Specs;

namespace FSH.Starter.WebApi.Store.Application.WarehouseLocations.Get.v1;

public sealed class GetWarehouseLocationHandler(
    ILogger<GetWarehouseLocationHandler> logger,
    [FromKeyedServices("store:warehouse-locations")] IRepository<WarehouseLocation> repository)
    : IRequestHandler<GetWarehouseLocationQuery, GetWarehouseLocationResponse>
{
    public async Task<GetWarehouseLocationResponse> Handle(GetWarehouseLocationQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var spec = new GetWarehouseLocationSpecification(request.Id);
        var warehouseLocation = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);
        _ = warehouseLocation ?? throw new WarehouseLocationNotFoundException(request.Id);
        
        logger.LogInformation("Retrieved warehouse location {WarehouseLocationId}", warehouseLocation.Id);
        
        return warehouseLocation.Adapt<GetWarehouseLocationResponse>();
    }
}
