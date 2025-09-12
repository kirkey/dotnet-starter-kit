namespace FSH.Starter.WebApi.Store.Application.Warehouses.Get.v1;

public record GetWarehouseQuery(DefaultIdType Id) : IRequest<GetWarehouseResponse>;
