using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.EventHandlers;

/// <summary>
/// Event handler that creates an inventory transaction when a reservation expires automatically.
/// </summary>
/// <remarks>
/// This handler maintains an audit trail of all automatic reservation expirations by creating
/// ADJUSTMENT type transactions with reason "RESERVATION_EXPIRED". This tracks when reservations
/// expire based on their expiration date and return inventory to available status.
/// </remarks>
public sealed class InventoryReservationExpiredHandler(
    ILogger<InventoryReservationExpiredHandler> logger,
    [FromKeyedServices("store:inventory-transactions")]
    IRepository<InventoryTransaction> transactionRepository)
    : INotificationHandler<InventoryReservationExpired>
{
    /// <summary>
    /// Handles the InventoryReservationExpired event by creating an inventory transaction record.
    /// </summary>
    /// <param name="notification">The domain event containing the expired reservation.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(InventoryReservationExpired notification, CancellationToken cancellationToken)
    {
        var reservation = notification.InventoryReservation;

        logger.LogInformation(
            "Processing inventory reservation expiration: {ReservationNumber} for Item {ItemId} at Warehouse {WarehouseId}: {Quantity} units",
            reservation.ReservationNumber,
            reservation.ItemId,
            reservation.WarehouseId,
            reservation.QuantityReserved);

        try
        {
            // Create an ADJUSTMENT transaction to record the expiration
            var transactionNumber = await GenerateTransactionNumber(cancellationToken);

            var transaction = InventoryTransaction.Create(
                transactionNumber: transactionNumber,
                itemId: reservation.ItemId,
                warehouseId: reservation.WarehouseId,
                warehouseLocationId: reservation.WarehouseLocationId,
                purchaseOrderId: null,
                transactionType: "ADJUSTMENT",
                reason: "RESERVATION_EXPIRED",
                quantity: reservation.QuantityReserved,
                quantityBefore: 0,
                unitCost: 0m,
                transactionDate: DateTime.UtcNow,
                reference: reservation.ReservationNumber,
                notes: $"Reservation {reservation.ReservationNumber} expired automatically. Expiration Date: {reservation.ExpirationDate:yyyy-MM-dd}",
                performedBy: "System",
                isApproved: true);

            await transactionRepository.AddAsync(transaction, cancellationToken);
            await transactionRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Created inventory transaction {TransactionNumber} for reservation expiration {ReservationNumber}",
                transactionNumber,
                reservation.ReservationNumber);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to create inventory transaction for reservation expiration: {ReservationNumber}",
                reservation.ReservationNumber);
            // Don't rethrow - event handlers should not break the main flow
        }
    }

    /// <summary>
    /// Generates a unique transaction number for reservation expirations.
    /// </summary>
    private async Task<string> GenerateTransactionNumber(CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var prefix = $"TXN-REXP-{date:yyyyMMdd}";
        
        // Get count of transactions with this prefix
        var count = await transactionRepository.CountAsync(cancellationToken);
        
        return $"{prefix}-{(count + 1):D6}";
    }
}

