namespace FSH.Starter.WebApi.Store.Application.Warehouses.Create.v1;

/// <summary>
/// Command to create a new warehouse.
/// </summary>
public record CreateWarehouseCommand : IRequest<CreateWarehouseResponse>
{
    /// <summary>
    /// Gets or sets the warehouse code.
    /// </summary>
    [DefaultValue("WH001")]
    public string Code { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the warehouse name.
    /// </summary>
    [DefaultValue("Main Warehouse")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the warehouse description.
    /// </summary>
    [DefaultValue("Primary storage facility")]
    public string? Description { get; init; }

    /// <summary>
    /// Gets or sets the warehouse address.
    /// </summary>
    [DefaultValue("123 Storage Street, New York, NY 10001, USA")]
    public string Address { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the manager name.
    /// </summary>
    [DefaultValue("John Manager")]
    public string ManagerName { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the manager email.
    /// </summary>
    [DefaultValue("john.manager@example.com")]
    public string ManagerEmail { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the manager phone.
    /// </summary>
    [DefaultValue("+1-555-123-4567")]
    public string ManagerPhone { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the total capacity.
    /// </summary>
    [DefaultValue(10000)]
    public decimal TotalCapacity { get; init; }

    /// <summary>
    /// Gets or sets the capacity unit.
    /// </summary>
    [DefaultValue("sqft")]
    public string CapacityUnit { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the warehouse type.
    /// </summary>
    [DefaultValue("Standard")]
    public string WarehouseType { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the warehouse is active.
    /// </summary>
    [DefaultValue(true)]
    public bool IsActive { get; init; }

    /// <summary>
    /// Gets or sets whether this is the main warehouse.
    /// </summary>
    [DefaultValue(false)]
    public bool IsMainWarehouse { get; init; }

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    [DefaultValue(null)]
    public string? Notes { get; init; }
}
