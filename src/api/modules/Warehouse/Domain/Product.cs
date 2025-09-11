using System.ComponentModel.DataAnnotations.Schema;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace FSH.Starter.WebApi.Warehouse.Domain;

public class Product : AuditableEntity, IAggregateRoot
{
    public string SKU { get; private set; } = string.Empty;
    public string Barcode { get; private set; } = string.Empty;
    public string Brand { get; private set; } = string.Empty;
    [Column(TypeName = "decimal(18,2)")]
    public decimal CostPrice { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal SellingPrice { get; private set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Weight { get; private set; }
    public string Unit { get; private set; } = string.Empty;
    public int ReorderLevel { get; private set; }
    public int MaxStockLevel { get; private set; }
    public bool IsPerishable { get; private set; }
    public int ShelfLifeDays { get; private set; }
    public bool RequiresBatchTracking { get; private set; }
    public bool IsActive { get; private set; } = true;

    public DefaultIdType CategoryId { get; private set; }
    public Category Category { get; private set; } = default!;

    public ICollection<ProductBatch> ProductBatches { get; private set; } = new List<ProductBatch>();
    public ICollection<WarehouseInventory> WarehouseInventories { get; private set; } = new List<WarehouseInventory>();
    public ICollection<StoreInventory> StoreInventories { get; private set; } = new List<StoreInventory>();
    public ICollection<SupplierProduct> SupplierProducts { get; private set; } = new List<SupplierProduct>();

    private Product() { }

    private Product(string name, string sku, string barcode, string brand, decimal costPrice, decimal sellingPrice, decimal weight, string unit, int reorderLevel, int maxStockLevel, bool isPerishable, int shelfLifeDays, bool requiresBatchTracking, bool isActive, DefaultIdType categoryId)
    {
        Name = name;
        SKU = sku;
        Barcode = barcode;
        Brand = brand;
        CostPrice = costPrice;
        SellingPrice = sellingPrice;
        Weight = weight;
        Unit = unit;
        ReorderLevel = reorderLevel;
        MaxStockLevel = maxStockLevel;
        IsPerishable = isPerishable;
        ShelfLifeDays = shelfLifeDays;
        RequiresBatchTracking = requiresBatchTracking;
        IsActive = isActive;
        CategoryId = categoryId;
    }

    public static Product Create(string name, string sku, string barcode, string brand, decimal costPrice, decimal sellingPrice, decimal weight, string unit, int reorderLevel, int maxStockLevel, bool isPerishable, int shelfLifeDays, bool requiresBatchTracking, bool isActive, DefaultIdType categoryId)
        => new(name, sku, barcode, brand, costPrice, sellingPrice, weight, unit, reorderLevel, maxStockLevel, isPerishable, shelfLifeDays, requiresBatchTracking, isActive, categoryId);

    public Product Update(string? name, string? sku, string? barcode, string? brand, decimal? costPrice, decimal? sellingPrice, decimal? weight, string? unit, int? reorderLevel, int? maxStockLevel, bool? isPerishable, int? shelfLifeDays, bool? requiresBatchTracking, bool? isActive, DefaultIdType? categoryId)
    {
        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.Ordinal)) Name = name;
        if (!string.IsNullOrWhiteSpace(sku) && !string.Equals(SKU, sku, StringComparison.Ordinal)) SKU = sku;
        if (!string.IsNullOrWhiteSpace(barcode) && !string.Equals(Barcode, barcode, StringComparison.Ordinal)) Barcode = barcode;
        if (!string.IsNullOrWhiteSpace(brand) && !string.Equals(Brand, brand, StringComparison.Ordinal)) Brand = brand;
        if (costPrice.HasValue) CostPrice = costPrice.Value;
        if (sellingPrice.HasValue) SellingPrice = sellingPrice.Value;
        if (weight.HasValue) Weight = weight.Value;
        if (unit is not null) Unit = unit;
        if (reorderLevel.HasValue) ReorderLevel = reorderLevel.Value;
        if (maxStockLevel.HasValue) MaxStockLevel = maxStockLevel.Value;
        if (isPerishable.HasValue) IsPerishable = isPerishable.Value;
        if (shelfLifeDays.HasValue) ShelfLifeDays = shelfLifeDays.Value;
        if (requiresBatchTracking.HasValue) RequiresBatchTracking = requiresBatchTracking.Value;
        if (isActive.HasValue) IsActive = isActive.Value;
        if (categoryId.HasValue && categoryId.Value != DefaultIdType.Empty) CategoryId = categoryId.Value;
        return this;
    }
}
