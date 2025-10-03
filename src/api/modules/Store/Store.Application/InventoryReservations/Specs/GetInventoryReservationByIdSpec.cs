namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.Specs;

public class GetInventoryReservationByIdSpec : Specification<InventoryReservation>
{
    public GetInventoryReservationByIdSpec(DefaultIdType id)
    {
        Query
            .Where(r => r.Id == id);
    }
}
