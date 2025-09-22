namespace FSH.Starter.WebApi.Store.Application.Warehouses.Search.v1;

public class SearchWarehousesCommand : PaginationFilter, IRequest<PagedList<WarehouseResponse>>
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsMainWarehouse { get; set; }
}
