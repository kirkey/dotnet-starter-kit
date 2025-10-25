using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.EventHandlers;

/// <summary>
/// Event handler that creates an inventory transaction when a reservation is cancelled.
/// </summary>
/// <remarks>
/// This handler maintains an audit trail of all reservation cancellations by creating
/// ADJUSTMENT type transactions with reason "RESERVATION_CANCELLED". This tracks when
/// reservations are manually cancelled before allocation or release.
/// </remarks>
public sealed class InventoryReservationCancelledHandler(
    ILogger<InventoryReservationCancelledHandler> logger,
    [FromKeyedServices("store:inventory-transactions")]
    IRepository<InventoryTransaction> transactionRepository)
    : INotificationHandler<InventoryReservationCancelled>
{
    /// <summary>
    /// Handles the InventoryReservationCancelled event by creating an inventory transaction record.
    /// </summary>
    /// <param name="notification">The domain event containing the cancelled reservation.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(InventoryReservationCancelled notification, CancellationToken cancellationToken)
    {
        var reservation = notification.InventoryReservation;

        logger.LogInformation(
            "Processing inventory reservation cancellation: {ReservationNumber} for Item {ItemId} at Warehouse {WarehouseId}: {Quantity} units, Reason: {Reason}",
            reservation.ReservationNumber,
            reservation.ItemId,
            reservation.WarehouseId,
            reservation.QuantityReserved,
            notification.Reason);

        try
        {
            // Create an ADJUSTMENT transaction to record the cancellation
            var transactionNumber = await GenerateTransactionNumber(cancellationToken);

            var transaction = InventoryTransaction.Create(
                transactionNumber: transactionNumber,
                itemId: reservation.ItemId,
                warehouseId: reservation.WarehouseId,
                warehouseLocationId: reservation.WarehouseLocationId,
                purchaseOrderId: null,
                transactionType: "ADJUSTMENT",
                reason: "RESERVATION_CANCELLED",
                quantity: reservation.QuantityReserved,
                quantityBefore: 0,
                unitCost: 0m,
                transactionDate: DateTime.UtcNow,
                reference: reservation.ReservationNumber,
                notes: $"Reservation {reservation.ReservationNumber} cancelled. Reason: {notification.Reason}",
                performedBy: null,
                isApproved: true);

            await transactionRepository.AddAsync(transaction, cancellationToken);
            await transactionRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Created inventory transaction {TransactionNumber} for reservation cancellation {ReservationNumber}",
                transactionNumber,
                reservation.ReservationNumber);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to create inventory transaction for reservation cancellation: {ReservationNumber}",
                reservation.ReservationNumber);
            // Don't rethrow - event handlers should not break the main flow
        }
    }

    /// <summary>
    /// Generates a unique transaction number for reservation cancellations.
    /// </summary>
    private async Task<string> GenerateTransactionNumber(CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var prefix = $"TXN-RCAN-{date:yyyyMMdd}";
        
        // Get count of transactions with this prefix
        var count = await transactionRepository.CountAsync(cancellationToken);
        
        return $"{prefix}-{(count + 1):D6}";
    }
}

