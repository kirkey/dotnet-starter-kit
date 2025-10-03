namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.Specs;

public class InventoryReservationByNumberSpec : Specification<InventoryReservation>
{
    public InventoryReservationByNumberSpec(string reservationNumber)
    {
        Query
            .Where(r => r.ReservationNumber == reservationNumber);
    }
}
