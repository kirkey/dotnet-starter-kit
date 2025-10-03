using FSH.Starter.WebApi.Store.Application.InventoryReservations.Specs;

namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.Search.v1;

/// <summary>
/// Handler for searching inventory reservations with filtering, sorting, and pagination support.
/// </summary>
/// <remarks>
/// Processes SearchInventoryReservationsCommand to retrieve a paginated list of inventory reservations
/// based on search criteria such as reservation number, item ID, warehouse ID, status, and date ranges.
/// Uses SearchInventoryReservationsSpec for building the query specification.
/// </remarks>
public sealed class SearchInventoryReservationsHandler(
    [FromKeyedServices("store:inventoryreservations")] IReadRepository<InventoryReservation> repository)
    : IRequestHandler<SearchInventoryReservationsCommand, PagedList<InventoryReservationResponse>>
{
    /// <summary>
    /// Handles the search request for inventory reservations.
    /// </summary>
    /// <param name="request">The search command containing filter criteria and pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A paginated list of inventory reservation responses.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    public async Task<PagedList<InventoryReservationResponse>> Handle(SearchInventoryReservationsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchInventoryReservationsSpec(request);

        var inventoryReservations = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var inventoryReservationResponses = inventoryReservations.Select(ir => new InventoryReservationResponse
        {
            Id = ir.Id,
            ReservationNumber = ir.ReservationNumber,
            ItemId = ir.ItemId,
            WarehouseId = ir.WarehouseId,
            WarehouseLocationId = ir.WarehouseLocationId,
            BinId = ir.BinId,
            LotNumberId = ir.LotNumberId,
            ReservedQuantity = ir.QuantityReserved,
            ReservationDate = ir.ReservationDate,
            ExpirationDate = ir.ExpirationDate,
            Status = ir.Status,
            ReferenceType = ir.ReservationType,
            ReferenceId = null, // This property doesn't exist in domain model, setting to null
            Notes = null // This property doesn't exist in domain model, setting to null
        }).ToList();

        return new PagedList<InventoryReservationResponse>(inventoryReservationResponses, request.PageNumber, request.PageSize, totalCount);
    }
}
