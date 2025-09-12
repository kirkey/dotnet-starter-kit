namespace FSH.Starter.WebApi.Store.Application.Warehouses.Get.v1;

public class GetWarehouseRequest(DefaultIdType id) : IRequest<WarehouseResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
