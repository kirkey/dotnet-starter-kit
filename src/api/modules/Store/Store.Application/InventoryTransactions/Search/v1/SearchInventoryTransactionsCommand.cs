using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Search.v1;

public class SearchInventoryTransactionsCommand : PaginationFilter, IRequest<PagedList<InventoryTransactionResponse>>
{
    public string? TransactionNumber { get; set; }
    public DefaultIdType? ItemId { get; set; }
    public DefaultIdType? WarehouseId { get; set; }
    public DefaultIdType? WarehouseLocationId { get; set; }
    public DefaultIdType? PurchaseOrderId { get; set; }
    public string? TransactionType { get; set; }
    public string? Reason { get; set; }
    public DateTime? TransactionDateFrom { get; set; }
    public DateTime? TransactionDateTo { get; set; }
    public bool? IsApproved { get; set; }
    public string? PerformedBy { get; set; }
    public string? ApprovedBy { get; set; }
    public decimal? MinTotalCost { get; set; }
    public decimal? MaxTotalCost { get; set; }
}

public class InventoryTransactionDto
{
    public DefaultIdType Id { get; set; }
    public string TransactionNumber { get; set; } = default!;
    public DefaultIdType ItemId { get; set; }
    public DefaultIdType? WarehouseId { get; set; }
    public DefaultIdType? WarehouseLocationId { get; set; }
    public string TransactionType { get; set; } = default!;
    public string Reason { get; set; } = default!;
    public int Quantity { get; set; }
    public int QuantityBefore { get; set; }
    public int QuantityAfter { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalCost { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Reference { get; set; }
    public bool IsApproved { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
}
