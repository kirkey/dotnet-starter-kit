namespace FSH.Starter.Blazor.Client.Pages.Warehouse.Locations;

/// <summary>
/// View model for warehouse location create/edit operations.
/// </summary>
public class WarehouseLocationViewModel
{
    /// <summary>
    /// Location identifier.
    /// </summary>
    public DefaultIdType? Id { get; set; }

    /// <summary>
    /// Location name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Location description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Location code.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Aisle identifier.
    /// </summary>
    public string? Aisle { get; set; }

    /// <summary>
    /// Section identifier.
    /// </summary>
    public string? Section { get; set; }

    /// <summary>
    /// Shelf identifier.
    /// </summary>
    public string? Shelf { get; set; }

    /// <summary>
    /// Bin identifier.
    /// </summary>
    public string? Bin { get; set; }

    /// <summary>
    /// Warehouse identifier.
    /// </summary>
    public DefaultIdType? WarehouseId { get; set; }

    /// <summary>
    /// Location type classification.
    /// </summary>
    public string? LocationType { get; set; }

    /// <summary>
    /// Storage capacity.
    /// </summary>
    public decimal Capacity { get; set; }

    /// <summary>
    /// Capacity unit of measurement.
    /// </summary>
    public string? CapacityUnit { get; set; }

    /// <summary>
    /// Whether the location is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Whether temperature control is required.
    /// </summary>
    public bool RequiresTemperatureControl { get; set; }

    /// <summary>
    /// Minimum temperature (if temperature controlled).
    /// </summary>
    public decimal? MinTemperature { get; set; }

    /// <summary>
    /// Maximum temperature (if temperature controlled).
    /// </summary>
    public decimal? MaxTemperature { get; set; }

    /// <summary>
    /// Temperature unit (C or F).
    /// </summary>
    public string? TemperatureUnit { get; set; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; set; }
}

