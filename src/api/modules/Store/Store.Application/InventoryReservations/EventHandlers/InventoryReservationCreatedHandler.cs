using Store.Domain.Events;

namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.EventHandlers;

/// <summary>
/// Event handler that creates an inventory transaction when a reservation is created.
/// </summary>
/// <remarks>
/// This handler maintains an audit trail of all inventory reservations by creating
/// ADJUSTMENT type transactions with reason "RESERVATION_CREATED". This provides
/// traceability for inventory availability and helps with reconciliation.
/// </remarks>
public sealed class InventoryReservationCreatedHandler : INotificationHandler<InventoryReservationCreated>
{
    private readonly ILogger<InventoryReservationCreatedHandler> _logger;
    private readonly IRepository<InventoryTransaction> _transactionRepository;

    public InventoryReservationCreatedHandler(
        ILogger<InventoryReservationCreatedHandler> logger,
        [FromKeyedServices("store:inventory-transactions")] IRepository<InventoryTransaction> transactionRepository)
    {
        _logger = logger;
        _transactionRepository = transactionRepository;
    }

    /// <summary>
    /// Handles the InventoryReservationCreated event by creating an inventory transaction record.
    /// </summary>
    /// <param name="notification">The domain event containing the created reservation.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(InventoryReservationCreated notification, CancellationToken cancellationToken)
    {
        var reservation = notification.InventoryReservation;

        _logger.LogInformation(
            "Processing inventory reservation creation: {ReservationNumber} for Item {ItemId} at Warehouse {WarehouseId}: {Quantity} units",
            reservation.ReservationNumber,
            reservation.ItemId,
            reservation.WarehouseId,
            reservation.QuantityReserved);

        try
        {
            // Create an ADJUSTMENT transaction to record the reservation
            var transactionNumber = await GenerateTransactionNumber(cancellationToken);

            var transaction = InventoryTransaction.Create(
                transactionNumber: transactionNumber,
                itemId: reservation.ItemId,
                warehouseId: reservation.WarehouseId,
                warehouseLocationId: reservation.WarehouseLocationId,
                purchaseOrderId: null,
                transactionType: "ADJUSTMENT",
                reason: "RESERVATION_CREATED",
                quantity: reservation.QuantityReserved,
                quantityBefore: 0, // We don't track before/after in reservations
                unitCost: 0m,
                transactionDate: DateTime.UtcNow,
                reference: reservation.ReservationNumber,
                notes: $"Reservation {reservation.ReservationNumber} created. Type: {reservation.ReservationType}, Reference: {reservation.ReferenceNumber ?? "N/A"}",
                performedBy: reservation.ReservedBy,
                isApproved: true);

            await _transactionRepository.AddAsync(transaction, cancellationToken);
            await _transactionRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Created inventory transaction {TransactionNumber} for reservation {ReservationNumber}",
                transactionNumber,
                reservation.ReservationNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to create inventory transaction for reservation: {ReservationNumber}",
                reservation.ReservationNumber);
            // Don't rethrow - event handlers should not break the main flow
        }
    }

    /// <summary>
    /// Generates a unique transaction number for reservations.
    /// </summary>
    private async Task<string> GenerateTransactionNumber(CancellationToken cancellationToken)
    {
        var date = DateTime.UtcNow;
        var prefix = $"TXN-RSV-{date:yyyyMMdd}";
        
        // Get count of transactions with this prefix
        var count = await _transactionRepository.CountAsync(cancellationToken);
        
        return $"{prefix}-{(count + 1):D6}";
    }
}

