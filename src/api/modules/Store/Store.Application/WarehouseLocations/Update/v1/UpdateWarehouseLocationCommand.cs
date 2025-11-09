namespace FSH.Starter.WebApi.Store.Application.WarehouseLocations.Update.v1;

/// <summary>
/// Command to update an existing warehouse location.
/// </summary>
public record UpdateWarehouseLocationCommand : IRequest<UpdateWarehouseLocationResponse>
{
    /// <summary>
    /// Gets or sets the warehouse location identifier.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Gets or sets the location name.
    /// </summary>
    [DefaultValue("Main Location")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the location description.
    /// </summary>
    [DefaultValue("Primary storage location")]
    public string? Description { get; init; }

    /// <summary>
    /// Gets or sets the location code.
    /// </summary>
    [DefaultValue("LOC001")]
    public string Code { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the aisle.
    /// </summary>
    [DefaultValue("A")]
    public string Aisle { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the section.
    /// </summary>
    [DefaultValue("01")]
    public string Section { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the shelf.
    /// </summary>
    [DefaultValue("01")]
    public string Shelf { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the bin.
    /// </summary>
    [DefaultValue("A")]
    public string? Bin { get; init; }

    /// <summary>
    /// Gets or sets the warehouse identifier.
    /// </summary>
    public DefaultIdType WarehouseId { get; init; }

    /// <summary>
    /// Gets or sets the location type.
    /// </summary>
    [DefaultValue("Floor")]
    public string LocationType { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the capacity.
    /// </summary>
    [DefaultValue(1000)]
    public decimal Capacity { get; init; }

    /// <summary>
    /// Gets or sets the capacity unit.
    /// </summary>
    [DefaultValue("sqft")]
    public string CapacityUnit { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the location is active.
    /// </summary>
    [DefaultValue(true)]
    public bool IsActive { get; init; }

    /// <summary>
    /// Gets or sets whether temperature control is required.
    /// </summary>
    [DefaultValue(false)]
    public bool RequiresTemperatureControl { get; init; }

    /// <summary>
    /// Gets or sets the minimum temperature.
    /// </summary>
    [DefaultValue(null)]
    public decimal? MinTemperature { get; init; }

    /// <summary>
    /// Gets or sets the maximum temperature.
    /// </summary>
    [DefaultValue(null)]
    public decimal? MaxTemperature { get; init; }

    /// <summary>
    /// Gets or sets the temperature unit.
    /// </summary>
    [DefaultValue("C")]
    public string? TemperatureUnit { get; init; }

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    public string? Notes { get; init; }
}
