using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using Store.Domain.Events;

namespace Store.Domain;

public sealed class WarehouseLocation : AuditableEntity, IAggregateRoot
{
    public string Code { get; private set; } = default!;
    public string Aisle { get; private set; } = default!;
    public string Section { get; private set; } = default!;
    public string Shelf { get; private set; } = default!;
    public string? Bin { get; private set; }
    public DefaultIdType WarehouseId { get; private set; }
    public string LocationType { get; private set; } = default!; // Floor, Rack, Cold Storage, etc.
    public decimal Capacity { get; private set; }
    public decimal UsedCapacity { get; private set; }
    public string CapacityUnit { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;
    public bool RequiresTemperatureControl { get; private set; }
    public decimal? MinTemperature { get; private set; }
    public decimal? MaxTemperature { get; private set; }
    public string? TemperatureUnit { get; private set; }
    
    public Warehouse Warehouse { get; private set; } = default!;
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
