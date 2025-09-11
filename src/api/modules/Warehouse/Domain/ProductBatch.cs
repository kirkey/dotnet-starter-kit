using System.ComponentModel.DataAnnotations.Schema;
using FSH.Framework.Core.Domain;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public class ProductBatch : AuditableEntity
{
    public string BatchNumber { get; private set; } = string.Empty;
    public string LotNumber { get; private set; } = string.Empty;
    public DateTime ManufactureDate { get; private set; }
    public DateTime ExpiryDate { get; private set; }
    public int InitialQuantity { get; private set; }
    public int RemainingQuantity { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal CostPerUnit { get; private set; }
    public bool IsActive { get; private set; } = true;

    public DefaultIdType ProductId { get; private set; }
    public Product Product { get; private set; } = default!;

    public DefaultIdType? PurchaseOrderDetailId { get; private set; }
    public PurchaseOrderDetail? PurchaseOrderDetail { get; private set; }

    public ICollection<InventoryTransaction> InventoryTransactions { get; private set; } = new List<InventoryTransaction>();
    public ICollection<SaleDetail> SaleDetails { get; private set; } = new List<SaleDetail>();

    private ProductBatch() { }

    public static ProductBatch Create(string batchNumber, string lotNumber, DateTime manufactureDate, DateTime expiryDate, int initialQuantity, decimal costPerUnit, DefaultIdType productId, DefaultIdType? purchaseOrderDetailId)
    {
        return new ProductBatch
        {
            BatchNumber = batchNumber,
            LotNumber = lotNumber,
            ManufactureDate = manufactureDate,
            ExpiryDate = expiryDate,
            InitialQuantity = initialQuantity,
            RemainingQuantity = initialQuantity,
            CostPerUnit = costPerUnit,
            ProductId = productId,
            PurchaseOrderDetailId = purchaseOrderDetailId
        };
    }

    public ProductBatch Update(string? batchNumber, string? lotNumber, DateTime? manufactureDate, DateTime? expiryDate, int? initialQuantity, int? remainingQuantity, decimal? costPerUnit, bool? isActive)
    {
        if (!string.IsNullOrWhiteSpace(batchNumber)) BatchNumber = batchNumber;
        if (!string.IsNullOrWhiteSpace(lotNumber)) LotNumber = lotNumber;
        if (manufactureDate.HasValue) ManufactureDate = manufactureDate.Value;
        if (expiryDate.HasValue) ExpiryDate = expiryDate.Value;
        if (initialQuantity.HasValue) InitialQuantity = initialQuantity.Value;
        if (remainingQuantity.HasValue) RemainingQuantity = remainingQuantity.Value;
        if (costPerUnit.HasValue) CostPerUnit = costPerUnit.Value;
        if (isActive.HasValue) IsActive = isActive.Value;
        return this;
    }
}

