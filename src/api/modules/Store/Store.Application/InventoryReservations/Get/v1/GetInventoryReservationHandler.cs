using FSH.Starter.WebApi.Store.Application.InventoryReservations.Specs;
using Store.Domain.Exceptions.InventoryReservation;

namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.Get.v1;

public class GetInventoryReservationHandler(
    [FromKeyedServices("store:inventoryreservations")] IReadRepository<InventoryReservation> repository)
    : IRequestHandler<GetInventoryReservationCommand, InventoryReservationResponse>
{
    public async Task<InventoryReservationResponse> Handle(GetInventoryReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await repository.FirstOrDefaultAsync(
            new GetInventoryReservationByIdSpec(request.Id),
            cancellationToken);

        if (reservation is null)
        {
            throw new InventoryReservationNotFoundException(request.Id);
        }

        return reservation.Adapt<InventoryReservationResponse>();
    }
}
