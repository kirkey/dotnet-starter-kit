using FSH.Starter.WebApi.Store.Application.Warehouses.Specs;

namespace FSH.Starter.WebApi.Store.Application.Warehouses.Get.v1;

public sealed class GetWarehouseHandler(
    [FromKeyedServices("store:warehouses")] IReadRepository<Warehouse> repository,
    ICacheService cache)
    : IRequestHandler<GetWarehouseCommand, WarehouseResponse>
{
    public async Task<WarehouseResponse> Handle(GetWarehouseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"warehouse:{request.Id}",
            async () =>
            {
                var spec = new GetWarehouseSpecs(request.Id);
                var response = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false) ?? 
                               throw new WarehouseNotFoundException(request.Id);
                return response;
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);
        return item!;
    }
}
