namespace FSH.Starter.WebApi.Store.Application.Warehouses.Get.v1;

/// <summary>
/// Request to get a warehouse by ID.
/// </summary>
public record GetWarehouseRequest(DefaultIdType Id) : IRequest<WarehouseResponse>;
