using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.EventHandlers;

/// <summary>
/// Event handler that creates an inventory transaction when a reservation is allocated to a pick list.
/// </summary>
/// <remarks>
/// This handler maintains an audit trail of all reservation allocations by creating
/// ADJUSTMENT type transactions with reason "RESERVATION_ALLOCATED". This tracks the
/// transition from soft reservation to hard allocation.
/// </remarks>
public sealed class InventoryReservationAllocatedHandler(
    ILogger<InventoryReservationAllocatedHandler> logger,
    [FromKeyedServices("store:inventory-transactions")]
    IRepository<InventoryTransaction> transactionRepository)
    : INotificationHandler<InventoryReservationAllocated>
{
    /// <summary>
    /// Handles the InventoryReservationAllocated event by creating an inventory transaction record.
    /// </summary>
    /// <param name="notification">The domain event containing the allocated reservation.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(InventoryReservationAllocated notification, CancellationToken cancellationToken)
    {
        var reservation = notification.InventoryReservation;

        logger.LogInformation(
            "Processing inventory reservation allocation: {ReservationNumber} for Item {ItemId} at Warehouse {WarehouseId}: {Quantity} units",
            reservation.ReservationNumber,
            reservation.ItemId,
            reservation.WarehouseId,
            reservation.QuantityReserved);

        try
        {
            // Create an ADJUSTMENT transaction to record the allocation
            var transactionNumber = await GenerateTransactionNumber(cancellationToken);

            var transaction = InventoryTransaction.Create(
                transactionNumber: transactionNumber,
                itemId: reservation.ItemId,
                warehouseId: reservation.WarehouseId,
                warehouseLocationId: reservation.WarehouseLocationId,
                purchaseOrderId: null,
                transactionType: "ADJUSTMENT",
                reason: "RESERVATION_ALLOCATED",
                quantity: reservation.QuantityReserved,
                quantityBefore: 0,
                unitCost: 0m,
                transactionDate: DateTime.UtcNow,
                reference: reservation.ReservationNumber,
                notes: $"Reservation {reservation.ReservationNumber} allocated to pick list. Type: {reservation.ReservationType}",
                performedBy: null,
                isApproved: true);

            await transactionRepository.AddAsync(transaction, cancellationToken);
            await transactionRepository.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Created inventory transaction {TransactionNumber} for reservation allocation {ReservationNumber}",
                transactionNumber,
                reservation.ReservationNumber);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to create inventory transaction for reservation allocation: {ReservationNumber}",
                reservation.ReservationNumber);
            // Don't rethrow - event handlers should not break the main flow
        }
    }

    /// <summary>
    /// Generates a unique transaction number for reservation allocations.
    /// </summary>
    private async Task<string> GenerateTransactionNumber(CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var prefix = $"TXN-RALC-{date:yyyyMMdd}";
        
        // Get count of transactions with this prefix
        var count = await transactionRepository.CountAsync(cancellationToken);
        
        return $"{prefix}-{(count + 1):D6}";
    }
}

