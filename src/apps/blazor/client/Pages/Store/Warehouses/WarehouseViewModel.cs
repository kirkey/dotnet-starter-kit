namespace FSH.Starter.Blazor.Client.Pages.Store.Warehouses;

/// <summary>
/// View model for warehouse create/edit operations.
/// </summary>
public class WarehouseViewModel
{
    /// <summary>
    /// Warehouse identifier.
    /// </summary>
    public DefaultIdType? Id { get; set; }

    /// <summary>
    /// Warehouse code.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Warehouse name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Warehouse description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Warehouse physical address.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Manager name.
    /// </summary>
    public string? ManagerName { get; set; }

    /// <summary>
    /// Manager email address.
    /// </summary>
    public string? ManagerEmail { get; set; }

    /// <summary>
    /// Manager phone number.
    /// </summary>
    public string? ManagerPhone { get; set; }

    /// <summary>
    /// Total storage capacity.
    /// </summary>
    public decimal TotalCapacity { get; set; }

    /// <summary>
    /// Capacity unit of measurement.
    /// </summary>
    public string? CapacityUnit { get; set; }

    /// <summary>
    /// Warehouse type classification.
    /// </summary>
    public string? WarehouseType { get; set; }

    /// <summary>
    /// Whether the warehouse is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Whether this is the main warehouse.
    /// </summary>
    public bool IsMainWarehouse { get; set; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; set; }
}

