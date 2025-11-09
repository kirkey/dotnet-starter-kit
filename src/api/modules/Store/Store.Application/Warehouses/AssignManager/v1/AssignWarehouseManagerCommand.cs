namespace FSH.Starter.WebApi.Store.Application.Warehouses.AssignManager.v1;

/// <summary>
/// Command to assign a new manager to a warehouse with proper validation and event tracking.
/// </summary>
public record AssignWarehouseManagerCommand : IRequest<AssignWarehouseManagerResponse>
{
    /// <summary>
    /// Gets or sets the warehouse identifier.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Gets or sets the manager name.
    /// </summary>
    [DefaultValue("Sarah Johnson")]
    public string ManagerName { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the manager email.
    /// </summary>
    [DefaultValue("sarah.johnson@example.com")]
    public string ManagerEmail { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the manager phone.
    /// </summary>
    [DefaultValue("+1-555-234-5678")]
    public string ManagerPhone { get; init; } = string.Empty;
}
