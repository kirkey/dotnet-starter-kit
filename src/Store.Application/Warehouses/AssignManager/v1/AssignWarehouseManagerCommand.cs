namespace FSH.Starter.WebApi.Store.Application.Warehouses.AssignManager.v1;

/// <summary>
/// Command to assign a new manager to a warehouse with proper validation and event tracking.
/// </summary>
public record AssignWarehouseManagerCommand(
    DefaultIdType Id,
    [property: DefaultValue("Sarah Johnson")] string ManagerName,
    [property: DefaultValue("sarah.johnson@example.com")] string ManagerEmail,
    [property: DefaultValue("+1-555-234-5678")] string ManagerPhone) : IRequest<AssignWarehouseManagerResponse>;
