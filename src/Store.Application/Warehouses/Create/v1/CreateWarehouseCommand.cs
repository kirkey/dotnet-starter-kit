namespace FSH.Starter.WebApi.Store.Application.Warehouses.Create.v1;

public record CreateWarehouseCommand(
    [property: DefaultValue("Main Warehouse")] string Name,
    [property: DefaultValue("Primary storage facility")] string? Description,
    [property: DefaultValue("WH001")] string Code,
    [property: DefaultValue("123 Storage Street")] string Address,
    [property: DefaultValue("New York")] string City,
    [property: DefaultValue("NY")] string? State,
    [property: DefaultValue("USA")] string Country,
    [property: DefaultValue("10001")] string? PostalCode,
    [property: DefaultValue("John Manager")] string ManagerName,
    [property: DefaultValue("john.manager@example.com")] string ManagerEmail,
    [property: DefaultValue("+1-555-123-4567")] string ManagerPhone,
    [property: DefaultValue(10000)] decimal TotalCapacity,
    [property: DefaultValue("sqft")] string CapacityUnit,
    [property: DefaultValue(true)] bool IsActive,
    [property: DefaultValue(false)] bool IsMainWarehouse) : IRequest<CreateWarehouseResponse>;
