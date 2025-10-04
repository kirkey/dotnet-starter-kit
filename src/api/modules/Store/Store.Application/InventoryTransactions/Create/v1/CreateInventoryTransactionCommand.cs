namespace FSH.Starter.WebApi.Store.Application.InventoryTransactions.Create.v1;

public class CreateInventoryTransactionCommand : IRequest<CreateInventoryTransactionResponse>
{
    public string TransactionNumber { get; set; } = default!;
    public DefaultIdType ItemId { get; set; }
    public DefaultIdType WarehouseId { get; set; }
    public DefaultIdType? WarehouseLocationId { get; set; }
    public DefaultIdType? PurchaseOrderId { get; set; }
    public string TransactionType { get; set; } = default!;
    public string Reason { get; set; } = default!;
    public int Quantity { get; set; }
    public int QuantityBefore { get; set; }
    public decimal UnitCost { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string? Reference { get; set; }
    public string? PerformedBy { get; set; }
    public bool IsApproved { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
}

public class CreateInventoryTransactionResponse(DefaultIdType id, string transactionNumber)
{
    public DefaultIdType Id { get; } = id;
    public string TransactionNumber { get; } = transactionNumber;
}
