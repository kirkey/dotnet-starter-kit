namespace Store.Domain.Exceptions.InventoryReservation;

/// <summary>
/// Exception thrown when an inventory reservation is not found.
/// </summary>
public class InventoryReservationNotFoundException : NotFoundException
{
    public InventoryReservationNotFoundException(DefaultIdType reservationId)
        : base($"Inventory reservation with ID '{reservationId}' was not found.")
    {
    }

    public InventoryReservationNotFoundException(string reservationNumber)
        : base($"Inventory reservation '{reservationNumber}' was not found.")
    {
    }
}

/// <summary>
/// Exception thrown when a reservation cannot be modified.
/// </summary>
public class InventoryReservationCannotBeModifiedException(DefaultIdType reservationId, string status)
    : BadRequestException($"Inventory reservation '{reservationId}' in status '{status}' cannot be modified.");

/// <summary>
/// Exception thrown when reservation status transition is invalid.
/// </summary>
public class InvalidInventoryReservationStatusException(string currentStatus, string attemptedStatus)
    : BadRequestException($"Cannot transition inventory reservation from '{currentStatus}' to '{attemptedStatus}'.");

/// <summary>
/// Exception thrown when trying to reserve more than available.
/// </summary>
public class InsufficientInventoryForReservationException(DefaultIdType itemId, int available, int requested)
    : BadRequestException($"Insufficient inventory for item '{itemId}'. Available: {available}, Requested: {requested}");
