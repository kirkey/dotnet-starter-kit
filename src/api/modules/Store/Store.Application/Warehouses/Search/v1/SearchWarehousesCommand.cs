namespace FSH.Starter.WebApi.Store.Application.Warehouses.Search.v1;

/// <summary>
/// Command for searching warehouses with pagination and filtering.
/// </summary>
public class SearchWarehousesCommand : PaginationFilter, IRequest<PagedList<WarehouseResponse>>
{
    /// <summary>
    /// Filter by warehouse name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Filter by warehouse code.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Filter by active status.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Filter by main warehouse flag.
    /// </summary>
    public bool? IsMainWarehouse { get; set; }
}
