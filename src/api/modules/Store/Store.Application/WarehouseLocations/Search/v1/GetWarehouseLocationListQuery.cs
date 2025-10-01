namespace FSH.Starter.WebApi.Store.Application.WarehouseLocations.Search.v1;

public record SearchWarehouseLocationsCommand(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    DefaultIdType? WarehouseId = null,
    string? LocationType = null,
    string? Aisle = null,
    bool? IsActive = null,
    bool? RequiresTemperatureControl = null) : IRequest<PagedList<GetWarehouseLocationListResponse>>;
