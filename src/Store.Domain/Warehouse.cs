namespace Store.Domain;

/// <summary>
/// Physical storage location (warehouse) that holds stock and locations.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track available capacity and used capacity.
/// - Identify the main warehouse vs satellite warehouses.
/// - Manage warehouse operations and staff assignments.
/// - Monitor inventory levels across multiple facilities.
/// - Support location-based reporting and analytics.
/// </remarks>
/// <seealso cref="Store.Domain.Events.WarehouseCreated"/>
/// <seealso cref="Store.Domain.Events.WarehouseUpdated"/>
/// <seealso cref="Store.Domain.Events.WarehouseCapacityUpdated"/>
/// <seealso cref="Store.Domain.Events.WarehouseInventoryCounted"/>
/// <seealso cref="Store.Domain.Events.WarehouseActivated"/>
/// <seealso cref="Store.Domain.Events.WarehouseDeactivated"/>
/// <seealso cref="Store.Domain.Exceptions.Warehouse.WarehouseNotFoundException"/>
/// <seealso cref="Store.Domain.Exceptions.Warehouse.WarehouseNotFoundByCodeException"/>
/// <seealso cref="Store.Domain.Exceptions.Warehouse.WarehouseInactiveException"/>
/// <seealso cref="Store.Domain.Exceptions.Warehouse.WarehouseCapacityExceededException"/>
public sealed class Warehouse : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Short warehouse code. Example: "WH-SEA", "WH-NYC".
    /// Max length: 50.
    /// </summary>
    public string Code { get; private set; } = default!;

    /// <summary>
    /// Street address for the warehouse.
    /// Example: "1234 Industrial Blvd". Max length: 500.
    /// </summary>
    public string Address { get; private set; } = default!;

    /// <summary>
    /// City where the warehouse is located.
    /// Example: "Seattle". Max length: 100.
    /// </summary>
    public string City { get; private set; } = default!;

    /// <summary>
    /// State where the warehouse is located (optional).
    /// Example: "WA". Max length: 100.
    /// </summary>
    public string? State { get; private set; }

    /// <summary>
    /// Country where the warehouse is located.
    /// Example: "US". Max length: 100.
    /// </summary>
    public string Country { get; private set; } = default!;

    /// <summary>
    /// Postal code for the warehouse address (optional).
    /// Example: "98101". Max length: 20.
    /// </summary>
    public string? PostalCode { get; private set; }

    /// <summary>
    /// Manager name responsible for warehouse operations.
    /// Example: "Sarah Johnson". Max length: 100.
    /// </summary>
    public string ManagerName { get; private set; } = default!;

    /// <summary>
    /// Manager email for warehouse communications.
    /// Example: "manager@warehouse.com". Max length: 255.
    /// </summary>
    public string ManagerEmail { get; private set; } = default!;

    /// <summary>
    /// Manager phone number for warehouse contact.
    /// Example: "+1-555-0300". Max length: 50.
    /// </summary>
    public string ManagerPhone { get; private set; } = default!;

    /// <summary>
    /// Total storage capacity expressed in a unit (e.g., sqft or pallets).
    /// Example: 50000.0 for 50,000 square feet. Must be &gt; 0.
    /// </summary>
    public decimal TotalCapacity { get; private set; }

    /// <summary>
    /// Used portion of the capacity. Default: 0.
    /// Example: 30000.0 for 30,000 square feet used.
    /// </summary>
    public decimal UsedCapacity { get; private set; }

    /// <summary>
    /// Unit used for capacity measurements. Default: "sqft".
    /// Example: "sqft", "pallets", "cubic_meters".
    /// </summary>
    public string CapacityUnit { get; private set; } = default!;

    /// <summary>
    /// Indicates if the warehouse is active.
    /// Default: true. Used to disable warehouses without deleting records.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Indicates if the warehouse is the main warehouse.
    /// Default: false. Only one warehouse should be marked as main.
    /// </summary>
    public bool IsMainWarehouse { get; private set; }

    /// <summary>
    /// Date of the last inventory count.
    /// Example: 2025-09-01T00:00:00Z. Updated when cycle counts are completed.
    /// </summary>
    public DateTime? LastInventoryDate { get; private set; }

    /// <summary>
    /// Collection of locations within the warehouse.
    /// Example: aisles, racks, bins for detailed inventory placement.
    /// </summary>
    public ICollection<WarehouseLocation> Locations { get; private set; } = new List<WarehouseLocation>();

    /// <summary>
    /// Collection of inventory transactions for the warehouse.
    /// Example: receipts, shipments, adjustments affecting this warehouse.
    /// </summary>
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

    /// <summary>
    /// Creates a new warehouse instance.
    /// </summary>
    /// <param name="name">Warehouse name.</param>
    /// <param name="description">Optional warehouse description.</param>
    /// <param name="code">Warehouse code.</param>
    /// <param name="address">Street address.</param>
    /// <param name="city">City.</param>
    /// <param name="state">State (optional).</param>
    /// <param name="country">Country.</param>
    /// <param name="postalCode">Postal code (optional).</param>
    /// <param name="managerName">Manager's full name.</param>
    /// <param name="managerEmail">Manager's email address.</param>
    /// <param name="managerPhone">Manager's phone number.</param>
    /// <param name="totalCapacity">Total storage capacity.</param>
    /// <param name="capacityUnit">Unit of measurement for capacity (default: "sqft").</param>
    /// <param name="isActive">Indicates if the warehouse is active (default: true).</param>
    /// <param name="isMainWarehouse">Indicates if this is the main warehouse (default: false).</param>
    /// <returns>A new <see cref="Warehouse"/> instance.</returns>
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

    /// <summary>
    /// Updates the warehouse details.
    /// </summary>
    /// <param name="name">New name for the warehouse.</param>
    /// <param name="description">New description.</param>
    /// <param name="address">New street address.</param>
    /// <param name="city">New city.</param>
    /// <param name="state">New state.</param>
    /// <param name="country">New country.</param>
    /// <param name="postalCode">New postal code.</param>
    /// <param name="managerName">New manager name.</param>
    /// <param name="managerEmail">New manager email.</param>
    /// <param name="managerPhone">New manager phone.</param>
    /// <param name="totalCapacity">New total capacity.</param>
    /// <param name="capacityUnit">New capacity unit.</param>
    /// <param name="isMainWarehouse">Indicates if this should be the main warehouse.</param>
    /// <returns>The updated <see cref="Warehouse"/> instance.</returns>
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

    /// <summary>
    /// Updates the capacity usage of the warehouse.
    /// </summary>
    /// <param name="usedCapacity">New used capacity value.</param>
    /// <returns>The updated <see cref="Warehouse"/> instance.</returns>
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

    /// <summary>
    /// Records the date of the last inventory count.
    /// </summary>
    /// <param name="inventoryDate">Date of the inventory count.</param>
    /// <returns>The updated <see cref="Warehouse"/> instance.</returns>
    public Warehouse RecordInventoryCount(DateTime inventoryDate)
    {
        LastInventoryDate = inventoryDate;
        QueueDomainEvent(new WarehouseInventoryCounted { Warehouse = this, InventoryDate = inventoryDate });
        return this;
    }

    /// <summary>
    /// Activates the warehouse, making it available for operations.
    /// </summary>
    /// <returns>The updated <see cref="Warehouse"/> instance.</returns>
    public Warehouse Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new WarehouseActivated { Warehouse = this });
        }
        return this;
    }

    /// <summary>
    /// Deactivates the warehouse, preventing its use in operations.
    /// </summary>
    /// <returns>The updated <see cref="Warehouse"/> instance.</returns>
    public Warehouse Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            QueueDomainEvent(new WarehouseDeactivated { Warehouse = this });
        }
        return this;
    }

    /// <summary>
    /// Calculates the percentage of capacity utilization.
    /// </summary>
    /// <returns>Capacity utilization percentage.</returns>
    public decimal GetCapacityUtilizationPercentage() => 
        TotalCapacity > 0 ? (UsedCapacity / TotalCapacity) * 100 : 0;

    /// <summary>
    /// Gets the available capacity remaining in the warehouse.
    /// </summary>
    /// <returns>Available capacity.</returns>
    public decimal GetAvailableCapacity() => Math.Max(0, TotalCapacity - UsedCapacity);

    /// <summary>
    /// Checks if the warehouse is near its capacity limit.
    /// </summary>
    /// <param name="thresholdPercentage">Threshold percentage to consider as near capacity (default: 90%).</param>
    /// <returns>True if near capacity, otherwise false.</returns>
    public bool IsNearCapacity(decimal thresholdPercentage = 90) => 
        GetCapacityUtilizationPercentage() >= thresholdPercentage;
}
