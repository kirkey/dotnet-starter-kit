namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.Create.v1;

public class CreateInventoryReservationCommand : IRequest<CreateInventoryReservationResponse>
{
    public string ReservationNumber { get; set; } = default!;
    public DefaultIdType ItemId { get; set; }
    public DefaultIdType WarehouseId { get; set; }
    public DefaultIdType? WarehouseLocationId { get; set; }
    public DefaultIdType? BinId { get; set; }
    public DefaultIdType? LotNumberId { get; set; }
    public int QuantityReserved { get; set; }
    public string ReservationType { get; set; } = default!;
    public string? ReferenceNumber { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? ReservedBy { get; set; }
}

public class CreateInventoryReservationResponse(DefaultIdType id, string reservationNumber)
{
    public DefaultIdType Id { get; } = id;
    public string ReservationNumber { get; } = reservationNumber;
}
