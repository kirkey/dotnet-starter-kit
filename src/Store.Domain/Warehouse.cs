namespace Store.Domain;

public sealed class Warehouse : AuditableEntity, IAggregateRoot
{
    public string Code { get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public string City { get; private set; } = default!;
    public string? State { get; private set; }
    public string Country { get; private set; } = default!;
    public string? PostalCode { get; private set; }
    public string ManagerName { get; private set; } = default!;
    public string ManagerEmail { get; private set; } = default!;
    public string ManagerPhone { get; private set; } = default!;
    public decimal TotalCapacity { get; private set; }
    public decimal UsedCapacity { get; private set; }
    public string CapacityUnit { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;
    public bool IsMainWarehouse { get; private set; }
    public DateTime? LastInventoryDate { get; private set; }
    
    public ICollection<WarehouseLocation> Locations { get; private set; } = new List<WarehouseLocation>();
    public ICollection<InventoryTransaction> InventoryTransactions { get; private set; } = new List<InventoryTransaction>();

    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private Warehouse() { }

    private Warehouse(
        DefaultIdType id,
        string name,
        string? description,
        string code,
        string address,
        string city,
        string? state,
        string country,
        string? postalCode,
        string managerName,
        string managerEmail,
        string managerPhone,
        decimal totalCapacity,
        string capacityUnit,
        bool isActive,
        bool isMainWarehouse)
    {
        // Domain validation
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
        if (name.Length > 200) throw new ArgumentException("Name must not exceed 200 characters", nameof(name));

        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code is required", nameof(code));
        if (code.Length > 50) throw new ArgumentException("Code must not exceed 50 characters", nameof(code));

        if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Address is required", nameof(address));
        if (address.Length > 500) throw new ArgumentException("Address must not exceed 500 characters", nameof(address));

        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("City is required", nameof(city));
        if (city.Length > 100) throw new ArgumentException("City must not exceed 100 characters", nameof(city));

        if (state is { Length: > 100 }) throw new ArgumentException("State must not exceed 100 characters", nameof(state));

        if (string.IsNullOrWhiteSpace(country)) throw new ArgumentException("Country is required", nameof(country));
        if (country.Length > 100) throw new ArgumentException("Country must not exceed 100 characters", nameof(country));

        if (postalCode is { Length: > 20 }) throw new ArgumentException("Postal code must not exceed 20 characters", nameof(postalCode));

        if (string.IsNullOrWhiteSpace(managerName)) throw new ArgumentException("Manager name is required", nameof(managerName));
        if (managerName.Length > 100) throw new ArgumentException("Manager name must not exceed 100 characters", nameof(managerName));

        if (string.IsNullOrWhiteSpace(managerEmail)) throw new ArgumentException("Manager email is required", nameof(managerEmail));
        if (managerEmail.Length > 255) throw new ArgumentException("Manager email must not exceed 255 characters", nameof(managerEmail));
        if (!EmailRegex.IsMatch(managerEmail)) throw new ArgumentException("Manager email format is invalid", nameof(managerEmail));

        if (string.IsNullOrWhiteSpace(managerPhone)) throw new ArgumentException("Manager phone is required", nameof(managerPhone));
        if (managerPhone.Length > 50) throw new ArgumentException("Manager phone must not exceed 50 characters", nameof(managerPhone));

        if (totalCapacity <= 0) throw new ArgumentException("Total capacity must be greater than 0", nameof(totalCapacity));
        if (string.IsNullOrWhiteSpace(capacityUnit)) throw new ArgumentException("Capacity unit is required", nameof(capacityUnit));
        if (capacityUnit.Length > 20) throw new ArgumentException("Capacity unit must not exceed 20 characters", nameof(capacityUnit));

        Id = id;
        Name = name;
        Description = description;
        Code = code;
        Address = address;
        City = city;
        State = state;
        Country = country;
        PostalCode = postalCode;
        ManagerName = managerName;
        ManagerEmail = managerEmail;
        ManagerPhone = managerPhone;
        TotalCapacity = totalCapacity;
        UsedCapacity = 0;
        CapacityUnit = capacityUnit;
        IsActive = isActive;
        IsMainWarehouse = isMainWarehouse;

        QueueDomainEvent(new WarehouseCreated { Warehouse = this });
    }

    public static Warehouse Create(
        string name,
        string? description,
        string code,
        string address,
        string city,
        string? state,
        string country,
        string? postalCode,
        string managerName,
        string managerEmail,
        string managerPhone,
        decimal totalCapacity,
        string capacityUnit = "sqft",
        bool isActive = true,
        bool isMainWarehouse = false)
    {
        return new Warehouse(
            DefaultIdType.NewGuid(),
            name,
            description,
            code,
            address,
            city,
            state,
            country,
            postalCode,
            managerName,
            managerEmail,
            managerPhone,
            totalCapacity,
            capacityUnit,
            isActive,
            isMainWarehouse);
    }

    public Warehouse Update(
        string? name,
        string? description,
        string? address,
        string? city,
        string? state,
        string? country,
        string? postalCode,
        string? managerName,
        string? managerEmail,
        string? managerPhone,
        decimal? totalCapacity,
        string? capacityUnit,
        bool? isMainWarehouse)
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

        if (!string.IsNullOrWhiteSpace(address) && !string.Equals(Address, address, StringComparison.OrdinalIgnoreCase))
        {
            Address = address;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(city) && !string.Equals(City, city, StringComparison.OrdinalIgnoreCase))
        {
            City = city;
            isUpdated = true;
        }

        if (!string.Equals(State, state, StringComparison.OrdinalIgnoreCase))
        {
            State = state;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(country) && !string.Equals(Country, country, StringComparison.OrdinalIgnoreCase))
        {
            Country = country;
            isUpdated = true;
        }

        if (!string.Equals(PostalCode, postalCode, StringComparison.OrdinalIgnoreCase))
        {
            PostalCode = postalCode;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(managerName) && !string.Equals(ManagerName, managerName, StringComparison.OrdinalIgnoreCase))
        {
            ManagerName = managerName;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(managerEmail) && !string.Equals(ManagerEmail, managerEmail, StringComparison.OrdinalIgnoreCase))
        {
            ManagerEmail = managerEmail;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(managerPhone) && !string.Equals(ManagerPhone, managerPhone, StringComparison.OrdinalIgnoreCase))
        {
            ManagerPhone = managerPhone;
            isUpdated = true;
        }

        if (totalCapacity.HasValue && TotalCapacity != totalCapacity.Value)
        {
            TotalCapacity = totalCapacity.Value;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(capacityUnit) && !string.Equals(CapacityUnit, capacityUnit, StringComparison.OrdinalIgnoreCase))
        {
            CapacityUnit = capacityUnit;
            isUpdated = true;
        }

        if (isMainWarehouse.HasValue && IsMainWarehouse != isMainWarehouse.Value)
        {
            IsMainWarehouse = isMainWarehouse.Value;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new WarehouseUpdated { Warehouse = this });
        }

        return this;
    }

    public Warehouse UpdateCapacityUsage(decimal usedCapacity)
    {
        if (UsedCapacity != usedCapacity)
        {
            var previousUsage = UsedCapacity;
            UsedCapacity = Math.Max(0, Math.Min(usedCapacity, TotalCapacity));
            
            QueueDomainEvent(new WarehouseCapacityUpdated 
            { 
                Warehouse = this, 
                PreviousUsage = previousUsage, 
                NewUsage = UsedCapacity 
            });
        }

        return this;
    }

    public Warehouse RecordInventoryCount(DateTime inventoryDate)
    {
        LastInventoryDate = inventoryDate;
        QueueDomainEvent(new WarehouseInventoryCounted { Warehouse = this, InventoryDate = inventoryDate });
        return this;
    }

    public Warehouse Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new WarehouseActivated { Warehouse = this });
        }
        return this;
    }

    public Warehouse Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            QueueDomainEvent(new WarehouseDeactivated { Warehouse = this });
        }
        return this;
    }

    public decimal GetCapacityUtilizationPercentage() => 
        TotalCapacity > 0 ? (UsedCapacity / TotalCapacity) * 100 : 0;

    public decimal GetAvailableCapacity() => Math.Max(0, TotalCapacity - UsedCapacity);

    public bool IsNearCapacity(decimal thresholdPercentage = 90) => 
        GetCapacityUtilizationPercentage() >= thresholdPercentage;
}
