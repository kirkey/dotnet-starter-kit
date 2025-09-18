

using FSH.Starter.WebApi.Store.Application.Warehouses.Specs;

namespace FSH.Starter.WebApi.Store.Application.Warehouses.Search.v1;

public sealed class SearchWarehousesHandler(
    [FromKeyedServices("store:warehouses")] IReadRepository<Warehouse> repository)
    : IRequestHandler<SearchWarehousesCommand, PagedList<WarehouseResponse>>
{
    public async Task<PagedList<WarehouseResponse>> Handle(SearchWarehousesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchWarehousesSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<WarehouseResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
