using FSH.Starter.WebApi.Store.Application.InventoryReservations.Specs;
using Store.Domain.Exceptions.InventoryReservation;

namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.Delete.v1;

public class DeleteInventoryReservationHandler(
    [FromKeyedServices("store:inventoryreservations")] IRepository<InventoryReservation> repository,
    [FromKeyedServices("store:inventoryreservations")] IReadRepository<InventoryReservation> readRepository)
    : IRequestHandler<DeleteInventoryReservationCommand, DeleteInventoryReservationResponse>
{
    public async Task<DeleteInventoryReservationResponse> Handle(DeleteInventoryReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await readRepository.FirstOrDefaultAsync(
            new GetInventoryReservationByIdSpec(request.Id),
            cancellationToken);

        if (reservation is null)
        {
            throw new InventoryReservationNotFoundException(request.Id);
        }

        await repository.DeleteAsync(reservation, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new DeleteInventoryReservationResponse(reservation.Id, reservation.ReservationNumber);
    }
}
