namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.Delete.v1;

public class DeleteInventoryReservationCommand : IRequest<DeleteInventoryReservationResponse>
{
    public DefaultIdType Id { get; set; }
}

public class DeleteInventoryReservationResponse(DefaultIdType id, string reservationNumber)
{
    public DefaultIdType Id { get; } = id;
    public string ReservationNumber { get; } = reservationNumber;
}
