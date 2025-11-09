namespace FSH.Starter.WebApi.Store.Application.WarehouseLocations.Search.v1;

/// <summary>
/// Command to search warehouse locations with filters and pagination.
/// </summary>
public class SearchWarehouseLocationsCommand : PaginationFilter, IRequest<PagedList<GetWarehouseLocationListResponse>>
{
    /// <summary>
    /// Optional search term to filter by code, name, or other text fields.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Optional warehouse ID to filter locations by warehouse.
    /// </summary>
    public DefaultIdType? WarehouseId { get; set; }

    /// <summary>
    /// Optional location type to filter (e.g., "Floor", "Rack", "Cold Storage").
    /// </summary>
    public string? LocationType { get; set; }

    /// <summary>
    /// Optional aisle to filter by aisle identifier.
    /// </summary>
    public string? Aisle { get; set; }

    /// <summary>
    /// Optional active status filter.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Optional temperature control requirement filter.
    /// </summary>
    public bool? RequiresTemperatureControl { get; set; }
}
