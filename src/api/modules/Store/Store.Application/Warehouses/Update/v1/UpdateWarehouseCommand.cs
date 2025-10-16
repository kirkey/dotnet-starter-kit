namespace FSH.Starter.WebApi.Store.Application.Warehouses.Update.v1;

public record UpdateWarehouseCommand(
    DefaultIdType Id,
    [property: DefaultValue("WH001")] string Code,
    [property: DefaultValue("Main Warehouse")] string Name,
    [property: DefaultValue("Primary storage facility")] string? Description,
    [property: DefaultValue("123 Storage Street, New York, NY 10001, USA")] string Address,
    [property: DefaultValue("John Manager")] string ManagerName,
    [property: DefaultValue("john.manager@example.com")] string ManagerEmail,
    [property: DefaultValue("+1-555-123-4567")] string ManagerPhone,
    [property: DefaultValue(10000)] decimal TotalCapacity,
    [property: DefaultValue("sqft")] string CapacityUnit,
    [property: DefaultValue("Standard")] string WarehouseType,
    [property: DefaultValue(true)] bool IsActive,
    [property: DefaultValue(false)] bool IsMainWarehouse,
    string? Notes) : IRequest<UpdateWarehouseResponse>;
