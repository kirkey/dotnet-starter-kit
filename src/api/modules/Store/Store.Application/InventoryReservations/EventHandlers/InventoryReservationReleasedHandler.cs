using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.EventHandlers;

/// <summary>
/// Event handler that creates an inventory transaction when a reservation is released.
/// </summary>
/// <remarks>
/// This handler maintains an audit trail of all reservation releases by creating
/// ADJUSTMENT type transactions with reason "RESERVATION_RELEASED". This tracks when
/// reserved inventory returns to available status.
/// </remarks>
public sealed class InventoryReservationReleasedHandler(
    ILogger<InventoryReservationReleasedHandler> logger,
    [FromKeyedServices("store:inventory-transactions")]
    IRepository<InventoryTransaction> transactionRepository)
    : INotificationHandler<InventoryReservationReleased>
{
    /// <summary>
    /// Handles the InventoryReservationReleased event by creating an inventory transaction record.
    /// </summary>
    /// <param name="notification">The domain event containing the released reservation.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(InventoryReservationReleased notification, CancellationToken cancellationToken)
    {
        var reservation = notification.InventoryReservation;

        logger.LogInformation(
            "Processing inventory reservation release: {ReservationNumber} for Item {ItemId} at Warehouse {WarehouseId}: {Quantity} units, Reason: {Reason}",
            reservation.ReservationNumber,
            reservation.ItemId,
            reservation.WarehouseId,
            reservation.QuantityReserved,
            notification.Reason);

        try
        {
            // Create an ADJUSTMENT transaction to record the release
            var transactionNumber = await GenerateTransactionNumber(cancellationToken);

            var transaction = InventoryTransaction.Create(
                transactionNumber: transactionNumber,
                itemId: reservation.ItemId,
                warehouseId: reservation.WarehouseId,
                warehouseLocationId: reservation.WarehouseLocationId,
                purchaseOrderId: null,
                transactionType: "ADJUSTMENT",
                reason: "RESERVATION_RELEASED",
                quantity: reservation.QuantityReserved,
                quantityBefore: 0,
                unitCost: 0m,
                transactionDate: DateTime.UtcNow,
                reference: reservation.ReservationNumber,
                notes: $"Reservation {reservation.ReservationNumber} released. Reason: {notification.Reason}",
                performedBy: null,
                isApproved: true);

            await transactionRepository.AddAsync(transaction, cancellationToken);
            await transactionRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Created inventory transaction {TransactionNumber} for reservation release {ReservationNumber}",
                transactionNumber,
                reservation.ReservationNumber);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to create inventory transaction for reservation release: {ReservationNumber}",
                reservation.ReservationNumber);
            // Don't rethrow - event handlers should not break the main flow
        }
    }

    /// <summary>
    /// Generates a unique transaction number for reservation releases.
    /// </summary>
    private async Task<string> GenerateTransactionNumber(CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var prefix = $"TXN-RREL-{date:yyyyMMdd}";
        
        // Get count of transactions with this prefix
        var count = await transactionRepository.CountAsync(cancellationToken);
        
        return $"{prefix}-{(count + 1):D6}";
    }
}

