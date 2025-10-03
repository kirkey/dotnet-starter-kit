using FSH.Starter.WebApi.Store.Application.InventoryReservations.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.Search.v1;

public class SearchInventoryReservationsCommand : PaginationFilter, IRequest<PagedList<InventoryReservationResponse>>
{
    public string? ReservationNumber { get; set; }
    public DefaultIdType? ItemId { get; set; }
    public DefaultIdType? WarehouseId { get; set; }
    public DefaultIdType? WarehouseLocationId { get; set; }
    public DefaultIdType? BinId { get; set; }
    public DefaultIdType? LotNumberId { get; set; }
    public string? ReservationType { get; set; }
    public string? Status { get; set; }
    public string? ReferenceNumber { get; set; }
    public DateTime? ReservationDateFrom { get; set; }
    public DateTime? ReservationDateTo { get; set; }
    public DateTime? ExpirationDateFrom { get; set; }
    public DateTime? ExpirationDateTo { get; set; }
    public bool? IsExpired { get; set; }
    public bool? IsActive { get; set; }
    public string? ReservedBy { get; set; }
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
