using System.ComponentModel.DataAnnotations.Schema;
using FSH.Framework.Core.Domain;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public class InventoryTransaction : AuditableEntity
{
    public TransactionType TransactionType { get; private set; }
    public int Quantity { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitCost { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalCost { get; private set; }
    public string Reference { get; private set; } = string.Empty;
    public string NotesText { get; private set; } = string.Empty;
    public DateTime TransactionDate { get; private set; }
    public string CreatedByName { get; private set; } = string.Empty;

    public DefaultIdType ProductId { get; private set; }
    public Product Product { get; private set; } = default!;
    public DefaultIdType? ProductBatchId { get; private set; }
    public ProductBatch? ProductBatch { get; private set; }
    public DefaultIdType? WarehouseId { get; private set; }
    public Warehouse? Warehouse { get; private set; }
    public DefaultIdType? StoreId { get; private set; }
    public Store? Store { get; private set; }

    private InventoryTransaction() { }

    public static InventoryTransaction Create(TransactionType type, int quantity, decimal unitCost, string reference, string? notes, DateTime date, string createdByName, DefaultIdType productId, DefaultIdType? productBatchId, DefaultIdType? warehouseId, DefaultIdType? storeId)
    {
        return new InventoryTransaction
        {
            TransactionType = type,
            Quantity = quantity,
            UnitCost = unitCost,
            TotalCost = unitCost * quantity,
            Reference = reference,
            NotesText = notes ?? string.Empty,
            TransactionDate = date,
            CreatedByName = createdByName,
            ProductId = productId,
            ProductBatchId = productBatchId,
            WarehouseId = warehouseId,
            StoreId = storeId
        };
    }
}

