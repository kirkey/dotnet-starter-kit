

using FSH.Starter.WebApi.Store.Application.Warehouses.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.Warehouses.Specs;

public class SearchWarehousesSpecs : EntitiesByPaginationFilterSpec<Warehouse, WarehouseResponse>
{
    public SearchWarehousesSpecs(SearchWarehousesCommand command)
        : base(command) =>
        Query
            .OrderBy(w => w.Name, !command.HasOrderBy())
            .Where(w => w.Name!.Contains(command.Name!), !string.IsNullOrEmpty(command.Name))
            .Where(w => w.Code == command.Code, !string.IsNullOrEmpty(command.Code))
            .Where(w => w.IsActive == command.IsActive, command.IsActive.HasValue)
            .Where(w => w.IsMainWarehouse == command.IsMainWarehouse, command.IsMainWarehouse.HasValue);
}
