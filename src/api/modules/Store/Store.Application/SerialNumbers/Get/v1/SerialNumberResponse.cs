namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Get.v1;

public class SerialNumberResponse
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
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
    public string? Notes { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
}
