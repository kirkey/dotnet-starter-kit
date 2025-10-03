namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.Get.v1;

/// <summary>
/// Command to get an inventory reservation by ID.
/// </summary>
public sealed record GetInventoryReservationCommand(DefaultIdType Id) : IRequest<InventoryReservationResponse>;
