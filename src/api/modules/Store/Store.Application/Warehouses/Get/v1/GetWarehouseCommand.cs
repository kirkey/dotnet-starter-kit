namespace FSH.Starter.WebApi.Store.Application.Warehouses.Get.v1;

/// <summary>
/// Command to get a warehouse by ID.
/// </summary>
public sealed record GetWarehouseCommand(DefaultIdType Id) : IRequest<WarehouseResponse>;
