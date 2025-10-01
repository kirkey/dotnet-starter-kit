namespace FSH.Starter.WebApi.Store.Application.Warehouses.Get.v1;

public sealed record WarehouseResponse(
    DefaultIdType Id, 
    string Name, 
    string? Description,
    string Code,
    string Address,
    string City,
    string? State,
    string Country,
    string? PostalCode,
    string ManagerName,
    string ManagerEmail,
    string ManagerPhone,
    decimal TotalCapacity,
    string CapacityUnit,
    string WarehouseType,
    bool IsActive,
    bool IsMainWarehouse);
