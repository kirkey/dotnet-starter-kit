namespace Store.Domain;

/// <summary>
/// Specific storage location inside a warehouse (aisle/section/shelf/bin).
/// </summary>
/// <remarks>
/// Use cases:
/// - Track where items are physically stored.
/// - Enforce temperature requirements for perishable goods.
/// - Support pick/pack operations with precise location data.
/// - Monitor location capacity and utilization.
/// - Enable zone-based inventory management.
/// </remarks>
/// <seealso cref="Store.Domain.Events.WarehouseLocationCreated"/>
/// <seealso cref="Store.Domain.Events.WarehouseLocationUpdated"/>
/// <seealso cref="Store.Domain.Events.WarehouseLocationCapacityUpdated"/>
/// <seealso cref="Store.Domain.Events.WarehouseLocationActivated"/>
/// <seealso cref="Store.Domain.Events.WarehouseLocationDeactivated"/>
/// <seealso cref="Store.Domain.Exceptions.WarehouseLocation.WarehouseLocationNotFoundException"/>
/// <seealso cref="Store.Domain.Exceptions.WarehouseLocation.WarehouseLocationNotFoundByCodeException"/>
/// <seealso cref="Store.Domain.Exceptions.WarehouseLocation.WarehouseLocationInactiveException"/>
/// <seealso cref="Store.Domain.Exceptions.WarehouseLocation.WarehouseLocationCapacityExceededException"/>
public sealed class WarehouseLocation : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Short location code. Example: "A1-S1-SH1", "COLD-01".
    /// Max length: 50.
    /// </summary>
    public string Code { get; private set; } = default!;

    /// <summary>
    /// Aisle identifier within the warehouse.
    /// Example: "A1", "A2", "COLD". Max length: 20.
    /// </summary>
    public string Aisle { get; private set; } = default!;

    /// <summary>
    /// Section identifier within the aisle.
    /// Example: "S1", "S2", "ZONE-A". Max length: 20.
    /// </summary>
    public string Section { get; private set; } = default!;

    /// <summary>
    /// Shelf identifier within the section.
    /// Example: "SH1", "TOP", "BOTTOM". Max length: 20.
    /// </summary>
    public string Shelf { get; private set; } = default!;

    /// <summary>
    /// Bin identifier within the shelf (optional).
    /// Example: "BIN-01", "LEFT", "RIGHT". Max length: 20.
    /// </summary>
    public string? Bin { get; private set; }

    /// <summary>
    /// The warehouse this location belongs to.
    /// Links to parent <see cref="Warehouse"/> for organization.
    /// </summary>
    public DefaultIdType WarehouseId { get; private set; }

    /// <summary>
    /// Type of location (e.g., Floor, Rack, Cold Storage).
    /// Example: "Floor", "Rack", "Cold Storage", "Freezer".
    /// </summary>
    public string LocationType { get; private set; } = default!;

    /// <summary>
    /// Capacity of this location (units) and the unit used.
    /// Example: 100.0 for 100 items or pallets. Must be &gt; 0.
    /// </summary>
    public decimal Capacity { get; private set; }

    /// <summary>
    /// Unit used for capacity measurement.
    /// Example: "items", "pallets", "cubic_feet". Default: "units".
    /// </summary>
    public string CapacityUnit { get; private set; } = default!;

    /// <summary>
    /// Used capacity of this location (units).
    /// Example: 75.0 for 75 items currently stored. Default: 0.
    /// </summary>
    public decimal UsedCapacity { get; private set; }

    /// <summary>
    /// Indicates if this location requires temperature control.
    /// Default: false. Set to true for cold storage, freezer zones.
    /// </summary>
    public bool RequiresTemperatureControl { get; private set; }

    /// <summary>
    /// Minimum temperature (if applicable).
    /// Example: 2.0 for 2°C in cold storage. Required if temperature control enabled.
    /// </summary>
    public decimal? MinTemperature { get; private set; }

    /// <summary>
    /// Maximum temperature (if applicable).
    /// Example: 8.0 for 8°C in cold storage. Required if temperature control enabled.
    /// </summary>
    public decimal? MaxTemperature { get; private set; }

    /// <summary>
    /// Unit of measurement for temperature (e.g., Celsius, Fahrenheit).
    /// Example: "C" for Celsius, "F" for Fahrenheit.
    /// </summary>
    public string? TemperatureUnit { get; private set; }

    /// <summary>
    /// Whether this warehouse location is active and usable.
    /// Default: true. Used to disable locations without deleting records.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Navigation property to the parent warehouse.
    /// </summary>
    public Warehouse Warehouse { get; private set; } = default!;

    /// <summary>
    /// Navigation property to grocery items stored in this location.
    /// </summary>
    public ICollection<GroceryItem> GroceryItems { get; private set; } = new List<GroceryItem>();

    private WarehouseLocation() { }

    private WarehouseLocation(
        DefaultIdType id,
        string name,
        string? description,
        string code,
        string aisle,
        string section,
        string shelf,
        string? bin,
        DefaultIdType warehouseId,
        string locationType,
        decimal capacity,
        string capacityUnit,
        bool isActive,
        bool requiresTemperatureControl,
        decimal? minTemperature,
        decimal? maxTemperature,
        string? temperatureUnit)
    {
        // domain validations
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
        if (name.Length > 200) throw new ArgumentException("Name must not exceed 200 characters", nameof(name));

        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code is required", nameof(code));
        if (code.Length > 50) throw new ArgumentException("Code must not exceed 50 characters", nameof(code));

        if (string.IsNullOrWhiteSpace(aisle)) throw new ArgumentException("Aisle is required", nameof(aisle));
        if (aisle.Length > 20) throw new ArgumentException("Aisle must not exceed 20 characters", nameof(aisle));

        if (string.IsNullOrWhiteSpace(section)) throw new ArgumentException("Section is required", nameof(section));
        if (section.Length > 20) throw new ArgumentException("Section must not exceed 20 characters", nameof(section));

        if (string.IsNullOrWhiteSpace(shelf)) throw new ArgumentException("Shelf is required", nameof(shelf));
        if (shelf.Length > 20) throw new ArgumentException("Shelf must not exceed 20 characters", nameof(shelf));

        if (bin is { Length: > 20 }) throw new ArgumentException("Bin must not exceed 20 characters", nameof(bin));

        if (capacity <= 0) throw new ArgumentException("Capacity must be greater than 0", nameof(capacity));
        if (string.IsNullOrWhiteSpace(capacityUnit)) throw new ArgumentException("CapacityUnit is required", nameof(capacityUnit));
        if (capacityUnit.Length > 20) throw new ArgumentException("CapacityUnit must not exceed 20 characters", nameof(capacityUnit));

        if (string.IsNullOrWhiteSpace(locationType)) throw new ArgumentException("LocationType is required", nameof(locationType));
        if (locationType.Length > 50) throw new ArgumentException("LocationType must not exceed 50 characters", nameof(locationType));

        if (requiresTemperatureControl)
        {
            if (!minTemperature.HasValue) throw new ArgumentException("MinTemperature is required when temperature control is enabled", nameof(minTemperature));
            if (!maxTemperature.HasValue) throw new ArgumentException("MaxTemperature is required when temperature control is enabled", nameof(maxTemperature));
            if (maxTemperature.Value <= minTemperature.Value) throw new ArgumentException("MaxTemperature must be greater than MinTemperature", nameof(maxTemperature));
            if (string.IsNullOrWhiteSpace(temperatureUnit)) throw new ArgumentException("TemperatureUnit is required when temperature control is enabled", nameof(temperatureUnit));
            if (!(temperatureUnit is "C" or "F")) throw new ArgumentException("TemperatureUnit must be 'C' or 'F'", nameof(temperatureUnit));
        }

        Id = id;
        Name = name;
        Description = description;
        Code = code;
        Aisle = aisle;
        Section = section;
        Shelf = shelf;
        Bin = bin;
        WarehouseId = warehouseId;
        LocationType = locationType;
        Capacity = capacity;
        UsedCapacity = 0;
        CapacityUnit = capacityUnit;
        IsActive = isActive;
        RequiresTemperatureControl = requiresTemperatureControl;
        MinTemperature = minTemperature;
        MaxTemperature = maxTemperature;
        TemperatureUnit = temperatureUnit;

        QueueDomainEvent(new WarehouseLocationCreated { WarehouseLocation = this });
    }

    public static WarehouseLocation Create(
        string name,
        string? description,
        string code,
        string aisle,
        string section,
        string shelf,
        string? bin,
        DefaultIdType warehouseId,
        string locationType,
        decimal capacity,
        string capacityUnit = "units",
        bool isActive = true,
        bool requiresTemperatureControl = false,
        decimal? minTemperature = null,
        decimal? maxTemperature = null,
        string? temperatureUnit = null)
    {
        return new WarehouseLocation(
            DefaultIdType.NewGuid(),
            name,
            description,
            code,
            aisle,
            section,
            shelf,
            bin,
            warehouseId,
            locationType,
            capacity,
            capacityUnit,
            isActive,
            requiresTemperatureControl,
            minTemperature,
            maxTemperature,
            temperatureUnit);
    }

    public WarehouseLocation Update(
        string? name,
        string? description,
        string? code,
        string? aisle,
        string? section,
        string? shelf,
        string? bin,
        DefaultIdType? warehouseId,
        string? locationType,
        decimal? capacity,
        string? capacityUnit,
        bool? isActive,
        bool? requiresTemperatureControl,
        decimal? minTemperature,
        decimal? maxTemperature,
        string? temperatureUnit)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
            isUpdated = true;
        }

        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(code) && !string.Equals(Code, code, StringComparison.OrdinalIgnoreCase))
        {
            Code = code;
            isUpdated = true;
        }

        if (warehouseId.HasValue && WarehouseId != warehouseId.Value)
        {
            WarehouseId = warehouseId.Value;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(aisle) && !string.Equals(Aisle, aisle, StringComparison.OrdinalIgnoreCase))
        {
            Aisle = aisle;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(section) && !string.Equals(Section, section, StringComparison.OrdinalIgnoreCase))
        {
            Section = section;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(shelf) && !string.Equals(Shelf, shelf, StringComparison.OrdinalIgnoreCase))
        {
            Shelf = shelf;
            isUpdated = true;
        }

        if (!string.Equals(Bin, bin, StringComparison.OrdinalIgnoreCase))
        {
            Bin = bin;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(locationType) && !string.Equals(LocationType, locationType, StringComparison.OrdinalIgnoreCase))
        {
            if (locationType.Length > 50) throw new ArgumentException("LocationType must not exceed 50 characters", nameof(locationType));
            LocationType = locationType;
            isUpdated = true;
        }

        if (capacity.HasValue && Capacity != capacity.Value)
        {
            Capacity = capacity.Value;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(capacityUnit) && !string.Equals(CapacityUnit, capacityUnit, StringComparison.OrdinalIgnoreCase))
        {
            CapacityUnit = capacityUnit;
            isUpdated = true;
        }

        if (isActive.HasValue && IsActive != isActive.Value)
        {
            IsActive = isActive.Value;
            isUpdated = true;
        }

        if (requiresTemperatureControl.HasValue && RequiresTemperatureControl != requiresTemperatureControl.Value)
        {
            RequiresTemperatureControl = requiresTemperatureControl.Value;
            isUpdated = true;
        }

        if (minTemperature != MinTemperature)
        {
            MinTemperature = minTemperature;
            isUpdated = true;
        }

        if (maxTemperature != MaxTemperature)
        {
            MaxTemperature = maxTemperature;
            isUpdated = true;
        }

        if (!string.Equals(TemperatureUnit, temperatureUnit, StringComparison.OrdinalIgnoreCase))
        {
            TemperatureUnit = temperatureUnit;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new WarehouseLocationUpdated { WarehouseLocation = this });
        }

        return this;
    }

    public WarehouseLocation UpdateCapacityUsage(decimal usedCapacity)
    {
        if (UsedCapacity != usedCapacity)
        {
            var previousUsage = UsedCapacity;
            UsedCapacity = Math.Max(0, Math.Min(usedCapacity, Capacity));
            
            QueueDomainEvent(new WarehouseLocationCapacityUpdated 
            { 
                WarehouseLocation = this, 
                PreviousUsage = previousUsage, 
                NewUsage = UsedCapacity 
            });
        }

        return this;
    }

    /// <summary>
    /// Update temperature control settings with validation and eventing.
    /// </summary>
    public WarehouseLocation UpdateTemperatureSettings(bool requiresTemperatureControl, decimal? minTemperature, decimal? maxTemperature, string? temperatureUnit)
    {
        if (requiresTemperatureControl)
        {
            if (!minTemperature.HasValue) throw new ArgumentException("MinTemperature is required when temperature control is enabled", nameof(minTemperature));
            if (!maxTemperature.HasValue) throw new ArgumentException("MaxTemperature is required when temperature control is enabled", nameof(maxTemperature));
            if (maxTemperature.Value <= minTemperature.Value) throw new ArgumentException("MaxTemperature must be greater than MinTemperature", nameof(maxTemperature));
            if (string.IsNullOrWhiteSpace(temperatureUnit)) throw new ArgumentException("TemperatureUnit is required when temperature control is enabled", nameof(temperatureUnit));
            if (!(temperatureUnit is "C" or "F")) throw new ArgumentException("TemperatureUnit must be 'C' or 'F'", nameof(temperatureUnit));
        }

        if (RequiresTemperatureControl != requiresTemperatureControl || MinTemperature != minTemperature || MaxTemperature != maxTemperature || !string.Equals(TemperatureUnit, temperatureUnit, StringComparison.OrdinalIgnoreCase))
        {
            RequiresTemperatureControl = requiresTemperatureControl;
            MinTemperature = minTemperature;
            MaxTemperature = maxTemperature;
            TemperatureUnit = temperatureUnit;
            QueueDomainEvent(new WarehouseLocationUpdated { WarehouseLocation = this });
        }

        return this;
    }

    /// <summary>
    /// Returns true if the location can be deactivated safely (no used capacity and no stored items).
    /// </summary>
    public bool CanBeDeactivated() => UsedCapacity == 0 && (GroceryItems == null || !GroceryItems.Any());

    /// <summary>
    /// Returns true if the location can be deleted safely (no used capacity and no stored items).
    /// </summary>
    public bool CanBeDeleted() => UsedCapacity == 0 && (GroceryItems == null || !GroceryItems.Any());

    public WarehouseLocation Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new WarehouseLocationActivated { WarehouseLocation = this });
        }
        return this;
    }

    public WarehouseLocation Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            QueueDomainEvent(new WarehouseLocationDeactivated { WarehouseLocation = this });
        }
        return this;
    }

    public decimal GetCapacityUtilizationPercentage() => 
        Capacity > 0 ? (UsedCapacity / Capacity) * 100 : 0;

    public decimal GetAvailableCapacity() => Math.Max(0, Capacity - UsedCapacity);

    public bool IsNearCapacity(decimal thresholdPercentage = 90) => 
        GetCapacityUtilizationPercentage() >= thresholdPercentage;

    public bool IsFull() => UsedCapacity >= Capacity;

    public string GetFullLocationCode() => $"{Aisle}-{Section}-{Shelf}" + (string.IsNullOrEmpty(Bin) ? "" : $"-{Bin}");
}
