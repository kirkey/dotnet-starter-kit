using FSH.Framework.Core.Exceptions;
using FSH.Starter.WebApi.Store.Application.InventoryReservations.Specs;

namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.Create.v1;

public class CreateInventoryReservationHandler(
    [FromKeyedServices("store:inventoryreservations")] IRepository<InventoryReservation> repository,
    [FromKeyedServices("store:inventoryreservations")] IReadRepository<InventoryReservation> readRepository)
    : IRequestHandler<CreateInventoryReservationCommand, CreateInventoryReservationResponse>
{
    public async Task<CreateInventoryReservationResponse> Handle(CreateInventoryReservationCommand request, CancellationToken cancellationToken)
    {
        // Check for duplicate reservation number
        var existingReservation = await readRepository.FirstOrDefaultAsync(
            new InventoryReservationByNumberSpec(request.ReservationNumber),
            cancellationToken);

        if (existingReservation is not null)
        {
            throw new ConflictException($"Reservation number '{request.ReservationNumber}' already exists.");
        }

        var reservation = InventoryReservation.Create(
            request.ReservationNumber,
            request.ItemId,
            request.WarehouseId,
            request.QuantityReserved,
            request.ReservationType,
            request.WarehouseLocationId,
            request.BinId,
            request.LotNumberId,
            request.ReferenceNumber,
            request.ExpirationDate,
            request.ReservedBy);

        if (!string.IsNullOrWhiteSpace(request.Name)) reservation.Name = request.Name;
        if (!string.IsNullOrWhiteSpace(request.Description)) reservation.Description = request.Description;
        if (!string.IsNullOrWhiteSpace(request.Notes)) reservation.Notes = request.Notes;

        await repository.AddAsync(reservation, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new CreateInventoryReservationResponse(reservation.Id, reservation.ReservationNumber);
    }
}
