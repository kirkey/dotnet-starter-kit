namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Search.v1;

public class SearchSerialNumbersCommand : PaginationFilter, IRequest<PagedList<SerialNumberResponse>>
{
    public string? SerialNumberValue { get; set; }
    public DefaultIdType? ItemId { get; set; }
    public DefaultIdType? WarehouseId { get; set; }
    public DefaultIdType? WarehouseLocationId { get; set; }
    public DefaultIdType? BinId { get; set; }
    public DefaultIdType? LotNumberId { get; set; }
    public string? Status { get; set; }
    public DateTime? ReceiptDateFrom { get; set; }
    public DateTime? ReceiptDateTo { get; set; }
    public bool? HasWarranty { get; set; }
    public bool? IsWarrantyExpired { get; set; }
    public string? ExternalReference { get; set; }
}

public class SerialNumberResponse
{
    public DefaultIdType Id { get; set; }
    public string SerialNumberValue { get; set; } = default!;
    public DefaultIdType ItemId { get; set; }
    public DefaultIdType? WarehouseId { get; set; }
    public DefaultIdType? WarehouseLocationId { get; set; }
    public DefaultIdType? BinId { get; set; }
    public DefaultIdType? LotNumberId { get; set; }
    public string Status { get; set; } = default!;
    public DateTime ReceiptDate { get; set; }
    public DateTime? ManufactureDate { get; set; }
    public DateTime? WarrantyExpirationDate { get; set; }
    public string? ExternalReference { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
}
