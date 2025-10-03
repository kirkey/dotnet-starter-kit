namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.Search.v1;

/// <summary>
/// Command to search inventory reservations with filtering and pagination.
/// </summary>
public class SearchInventoryReservationsCommand : PaginationFilter, IRequest<PagedList<InventoryReservationResponse>>
{
    public string? ReservationNumber { get; init; }
    public DefaultIdType? ItemId { get; init; }
    public DefaultIdType? WarehouseId { get; init; }
    public DefaultIdType? WarehouseLocationId { get; init; }
    public DefaultIdType? BinId { get; init; }
    public DefaultIdType? LotNumberId { get; init; }
    public string? ReservationType { get; init; }
    public string? Status { get; init; }
    public string? ReferenceNumber { get; init; }
    public DateTime? ReservationDateFrom { get; init; }
    public DateTime? ReservationDateTo { get; init; }
    public DateTime? ExpirationDateFrom { get; init; }
    public DateTime? ExpirationDateTo { get; init; }
    public bool? IsExpired { get; init; }
    public bool? IsActive { get; init; }
    public string? ReservedBy { get; init; }
}

public class InventoryReservationDto
{
    public DefaultIdType Id { get; set; }
    public string ReservationNumber { get; set; } = default!;
    public DefaultIdType ItemId { get; set; }
    public DefaultIdType WarehouseId { get; set; }
    public DefaultIdType? WarehouseLocationId { get; set; }
    public DefaultIdType? BinId { get; set; }
    public DefaultIdType? LotNumberId { get; set; }
    public int QuantityReserved { get; set; }
    public string ReservationType { get; set; } = default!;
    public string Status { get; set; } = default!;
    public string? ReferenceNumber { get; set; }
    public DateTime ReservationDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public string? ReservedBy { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
}
