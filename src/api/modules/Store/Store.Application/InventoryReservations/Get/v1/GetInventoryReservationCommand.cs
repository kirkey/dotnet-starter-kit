namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.Get.v1;

public class GetInventoryReservationCommand : IRequest<InventoryReservationResponse>
{
    public DefaultIdType Id { get; set; }
}
