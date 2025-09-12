namespace FSH.Starter.WebApi.Store.Application.Warehouses.Get.v1;

public record GetWarehouseResponse(
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
    decimal UsedCapacity,
    string CapacityUnit,
    bool IsActive,
    bool IsMainWarehouse,
    DateTime? LastInventoryDate,
    DateTime CreatedOn,
    DateTime? LastModifiedOn);
