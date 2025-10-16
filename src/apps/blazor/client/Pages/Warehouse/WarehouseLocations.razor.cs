using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.Warehouse;

/// <summary>
/// Warehouse Locations page component for managing specific storage locations within warehouses.
/// Provides CRUD operations for warehouse location entities including location hierarchy,
/// capacity tracking, and temperature control management.
/// </summary>
public partial class WarehouseLocations
{
    protected EntityServerTableContext<GetWarehouseLocationListResponse, DefaultIdType, WarehouseLocationViewModel> Context { get; set; } = default!;

    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<GetWarehouseLocationListResponse, DefaultIdType, WarehouseLocationViewModel>(
            entityName: "Warehouse Location",
            entityNamePlural: "Warehouse Locations",
            entityResource: FshResources.Warehouse,
            fields:
            [
                new EntityField<GetWarehouseLocationListResponse>(response => response.Code, "Code", "Code"),
                new EntityField<GetWarehouseLocationListResponse>(response => response.Name, "Name", "Name"),
                new EntityField<GetWarehouseLocationListResponse>(response => response.WarehouseName, "Warehouse", "WarehouseName"),
                new EntityField<GetWarehouseLocationListResponse>(response => response.Aisle, "Aisle", "Aisle"),
                new EntityField<GetWarehouseLocationListResponse>(response => response.Section, "Section", "Section"),
                new EntityField<GetWarehouseLocationListResponse>(response => response.Shelf, "Shelf", "Shelf"),
                new EntityField<GetWarehouseLocationListResponse>(response => response.LocationType, "Type", "LocationType"),
                new EntityField<GetWarehouseLocationListResponse>(response => response.Capacity, "Capacity", "Capacity", typeof(decimal)),
                new EntityField<GetWarehouseLocationListResponse>(response => response.CapacityUnit, "Unit", "CapacityUnit"),
                new EntityField<GetWarehouseLocationListResponse>(response => response.RequiresTemperatureControl, "Temp Control", "RequiresTemperatureControl", typeof(bool)),
                new EntityField<GetWarehouseLocationListResponse>(response => response.IsActive, "Active", "IsActive", typeof(bool))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<SearchWarehouseLocationsCommand>();

                var result = await Client.SearchWarehouseLocationsEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<GetWarehouseLocationListResponse>>();
            },
            createFunc: async location =>
            {
                await Client.CreateWarehouseLocationEndpointAsync("1", location.Adapt<CreateWarehouseLocationCommand>());
            },
            updateFunc: async (id, location) =>
            {
                await Client.UpdateWarehouseLocationEndpointAsync("1", id, location.Adapt<UpdateWarehouseLocationCommand>());
            },
            deleteFunc: async id => await Client.DeleteWarehouseLocationEndpointAsync("1", id));
        
        return Task.CompletedTask;
    }
}

/// <summary>
/// View model for warehouse location form operations.
/// Includes validation attributes and data annotations for comprehensive warehouse location management.
/// </summary>
public class WarehouseLocationViewModel
{
    /// <summary>
    /// Warehouse location unique identifier.
    /// </summary>
    public DefaultIdType? Id { get; set; }

    /// <summary>
    /// Location name for identification and display purposes.
    /// Required field with maximum length of 200 characters.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = default!;

    /// <summary>
    /// Optional description providing additional location details.
    /// Maximum length of 1000 characters.
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Unique location code for identification and referencing.
    /// Required field with maximum length of 50 characters.
    /// Examples: "A1-S1-SH1", "LOC001", "COLD-01"
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = default!;

    /// <summary>
    /// Aisle identifier within the warehouse.
    /// Required field with maximum length of 20 characters.
    /// Examples: "A1", "A2", "COLD"
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Aisle { get; set; } = default!;

    /// <summary>
    /// Section identifier within the aisle.
    /// Required field with maximum length of 20 characters.
    /// Examples: "S1", "S2", "ZONE-A"
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Section { get; set; } = default!;

    /// <summary>
    /// Shelf identifier within the section.
    /// Required field with maximum length of 20 characters.
    /// Examples: "SH1", "TOP", "BOTTOM"
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Shelf { get; set; } = default!;

    /// <summary>
    /// Bin identifier within the shelf (optional).
    /// Optional field with maximum length of 20 characters.
    /// Examples: "BIN-01", "LEFT", "RIGHT"
    /// </summary>
    [MaxLength(20)]
    public string? Bin { get; set; }

    /// <summary>
    /// The warehouse this location belongs to.
    /// Required foreign key reference to parent warehouse.
    /// </summary>
    [Required]
    public DefaultIdType? WarehouseId { get; set; }

    /// <summary>
    /// Type of location indicating storage capabilities and characteristics.
    /// Required field with maximum length of 50 characters.
    /// Examples: "Floor", "Rack", "Cold Storage", "Freezer"
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string LocationType { get; set; } = "Floor";

    /// <summary>
    /// Storage capacity of this location.
    /// Must be greater than 0.
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Capacity must be greater than 0")]
    public decimal Capacity { get; set; }

    /// <summary>
    /// Unit of measurement for capacity.
    /// Required field with maximum length of 20 characters.
    /// Default value is "units".
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string CapacityUnit { get; set; } = "units";

    /// <summary>
    /// Currently used capacity of this location.
    /// Read-only field updated by inventory movements.
    /// </summary>
    public decimal UsedCapacity { get; set; }

    /// <summary>
    /// Indicates whether this location requires temperature control.
    /// Default value is false.
    /// </summary>
    public bool RequiresTemperatureControl { get; set; }

    /// <summary>
    /// Minimum temperature requirement (if temperature control is enabled).
    /// Required if RequiresTemperatureControl is true.
    /// </summary>
    public decimal? MinTemperature { get; set; }

    /// <summary>
    /// Maximum temperature requirement (if temperature control is enabled).
    /// Required if RequiresTemperatureControl is true.
    /// </summary>
    public decimal? MaxTemperature { get; set; }

    /// <summary>
    /// Unit of measurement for temperature.
    /// Required if RequiresTemperatureControl is true.
    /// Maximum length of 5 characters.
    /// </summary>
    [MaxLength(5)]
    public string? TemperatureUnit { get; set; } = "C";

    /// <summary>
    /// Indicates whether the location is active and operational.
    /// Default value is true.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
