namespace FSH.Starter.WebApi.Store.Application.WarehouseLocations.Get.v1;

public record GetWarehouseLocationQuery(DefaultIdType Id) : IRequest<GetWarehouseLocationResponse>;
