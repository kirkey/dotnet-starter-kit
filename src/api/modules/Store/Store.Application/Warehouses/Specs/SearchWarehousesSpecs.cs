using FSH.Starter.WebApi.Store.Application.Warehouses.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.Warehouses.Specs;

/// <summary>
/// Specification for searching warehouses with filtering and pagination.
/// </summary>
public class SearchWarehousesSpecs : EntitiesByPaginationFilterSpec<Warehouse, WarehouseResponse>
{
    public SearchWarehousesSpecs(SearchWarehousesRequest request)
        : base(request) =>
        Query
            .OrderBy(w => w.Name, !request.HasOrderBy())
            .Where(w => w.Name!.Contains(request.Name!), !string.IsNullOrEmpty(request.Name))
            .Where(w => w.Code == request.Code, !string.IsNullOrEmpty(request.Code))
            .Where(w => w.IsActive == request.IsActive, request.IsActive.HasValue)
            .Where(w => w.IsMainWarehouse == request.IsMainWarehouse, request.IsMainWarehouse.HasValue);
}
