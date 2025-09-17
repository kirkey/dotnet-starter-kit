namespace Store.Domain;

/// <summary>
/// Represents a product (grocery item) stored and sold by the store.
/// Contains pricing, stock and identification fields.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track stock levels and reorder points.
/// - Provide pricing and weight details for orders.
/// </remarks>
public sealed class GroceryItem : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Stock keeping unit: short unique identifier. Example: "SKU-1234".
    /// </summary>
    public string SKU { get; private set; } = default!;

    /// <summary>
    /// Product barcode. Example: "0123456789012".
    /// </summary>
    public string Barcode { get; private set; } = default!;

    /// <summary>
    /// Selling price per unit. Default must be >= 0.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// Supplier cost per unit. Default must be >= 0.
    /// </summary>
    public decimal Cost { get; private set; }

    /// <summary>
    /// Minimum stock to keep (safety stock). Example: 5.
    /// </summary>
    public int MinimumStock { get; private set; }

    /// <summary>
    /// Maximum allowed stock (optional). Example: 100.
    /// </summary>
    public int MaximumStock { get; private set; }

    /// <summary>
    /// Current available stock quantity. Default >= 0.
    /// </summary>
    public int CurrentStock { get; private set; }

    /// <summary>
    /// Reorder point that triggers procurement. Example: 10.
    /// </summary>
    public int ReorderPoint { get; private set; }

    /// <summary>
    /// Whether the item is perishable (affects expiry tracking).
    /// </summary>
    public bool IsPerishable { get; private set; }

    /// <summary>
    /// Optional expiry date when the item is perishable.
    /// </summary>
    public DateTime? ExpiryDate { get; private set; }
    public string? Brand { get; private set; }
    public string? Manufacturer { get; private set; }
    public decimal Weight { get; private set; }
    public string? WeightUnit { get; private set; }
    public DefaultIdType? CategoryId { get; private set; }
    public DefaultIdType? SupplierId { get; private set; }
    public DefaultIdType? WarehouseLocationId { get; private set; }
    
    public Category Category { get; private set; } = default!;
    public Supplier Supplier { get; private set; } = default!;
    public WarehouseLocation? WarehouseLocation { get; private set; }

    private GroceryItem() { }

    private GroceryItem(
        DefaultIdType id,
        string name,
        string? description,
        string sku,
        string barcode,
        decimal price,
        decimal cost,
        int minimumStock,
        int maximumStock,
        int currentStock,
        int reorderPoint,
        bool isPerishable,
        DateTime? expiryDate,
        string? brand,
        string? manufacturer,
        decimal weight,
        string? weightUnit,
        DefaultIdType? categoryId,
        DefaultIdType? supplierId,
        DefaultIdType? warehouseLocationId)
    {
        // domain validations
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
        if (name.Length > 200) throw new ArgumentException("Name must not exceed 200 characters", nameof(name));

        if (string.IsNullOrWhiteSpace(sku)) throw new ArgumentException("SKU is required", nameof(sku));
        if (sku.Length > 100) throw new ArgumentException("SKU must not exceed 100 characters", nameof(sku));

        if (string.IsNullOrWhiteSpace(barcode)) throw new ArgumentException("Barcode is required", nameof(barcode));
        if (barcode.Length > 100) throw new ArgumentException("Barcode must not exceed 100 characters", nameof(barcode));

        if (price < 0m) throw new ArgumentException("Price must be zero or greater", nameof(price));
        if (cost < 0m) throw new ArgumentException("Cost must be zero or greater", nameof(cost));

        if (minimumStock < 0) throw new ArgumentException("MinimumStock must be zero or greater", nameof(minimumStock));
        if (maximumStock < 0) throw new ArgumentException("MaximumStock must be zero or greater", nameof(maximumStock));
        if (maximumStock > 0 && minimumStock > maximumStock) throw new ArgumentException("MinimumStock cannot be greater than MaximumStock", nameof(minimumStock));

        if (currentStock < 0) throw new ArgumentException("CurrentStock must be zero or greater", nameof(currentStock));
        if (maximumStock > 0 && currentStock > maximumStock) throw new ArgumentException("CurrentStock cannot exceed MaximumStock", nameof(currentStock));

        if (reorderPoint < 0) throw new ArgumentException("ReorderPoint must be zero or greater", nameof(reorderPoint));

        if (weight < 0m) throw new ArgumentException("Weight must be zero or greater", nameof(weight));
        if (weightUnit is { Length: > 20 }) throw new ArgumentException("WeightUnit must not exceed 20 characters", nameof(weightUnit));

        if (brand is { Length: > 200 }) throw new ArgumentException("Brand must not exceed 200 characters", nameof(brand));
        if (manufacturer is { Length: > 200 }) throw new ArgumentException("Manufacturer must not exceed 200 characters", nameof(manufacturer));

        Id = id;
        Name = name;
        Description = description;
        SKU = sku;
        Barcode = barcode;
        Price = price;
        Cost = cost;
        MinimumStock = minimumStock;
        MaximumStock = maximumStock;
        CurrentStock = currentStock;
        ReorderPoint = reorderPoint;
        IsPerishable = isPerishable;
        ExpiryDate = expiryDate;
        Brand = brand;
        Manufacturer = manufacturer;
        Weight = weight;
        WeightUnit = weightUnit;
        CategoryId = categoryId;
        SupplierId = supplierId;
        WarehouseLocationId = warehouseLocationId;

        QueueDomainEvent(new GroceryItemCreated { GroceryItem = this });
    }

    public static GroceryItem Create(
        string name,
        string? description,
        string sku,
        string barcode,
        decimal price,
        decimal cost,
        int minimumStock,
        int maximumStock,
        int currentStock,
        int reorderPoint,
        bool isPerishable,
        DateTime? expiryDate,
        string? brand,
        string? manufacturer,
        decimal weight,
        string? weightUnit,
        DefaultIdType? categoryId,
        DefaultIdType? supplierId,
        DefaultIdType? warehouseLocationId = null)
    {
        return new GroceryItem(
            DefaultIdType.NewGuid(),
            name,
            description,
            sku,
            barcode,
            price,
            cost,
            minimumStock,
            maximumStock,
            currentStock,
            reorderPoint,
            isPerishable,
            expiryDate,
            brand,
            manufacturer,
            weight,
            weightUnit,
            categoryId,
            supplierId,
            warehouseLocationId);
    }

    public GroceryItem UpdateDetails(
        string? name,
        string? description,
        decimal? price,
        decimal? cost,
        string? brand,
        string? manufacturer,
        decimal? weight,
        string? weightUnit,
        DefaultIdType? categoryId,
        DefaultIdType? supplierId)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
            isUpdated = true;
        }

        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
            isUpdated = true;
        }

        if (price.HasValue && Price != price.Value)
        {
            Price = price.Value;
            isUpdated = true;
        }

        if (cost.HasValue && Cost != cost.Value)
        {
            Cost = cost.Value;
            isUpdated = true;
        }

        if (!string.Equals(Brand, brand, StringComparison.OrdinalIgnoreCase))
        {
            Brand = brand;
            isUpdated = true;
        }

        if (!string.Equals(Manufacturer, manufacturer, StringComparison.OrdinalIgnoreCase))
        {
            Manufacturer = manufacturer;
            isUpdated = true;
        }

        if (weight.HasValue && Weight != weight.Value)
        {
            Weight = weight.Value;
            isUpdated = true;
        }

        if (!string.Equals(WeightUnit, weightUnit, StringComparison.OrdinalIgnoreCase))
        {
            WeightUnit = weightUnit;
            isUpdated = true;
        }

        if (categoryId.HasValue && CategoryId != categoryId.Value)
        {
            CategoryId = categoryId.Value;
            isUpdated = true;
        }

        if (supplierId.HasValue && SupplierId != supplierId.Value)
        {
            SupplierId = supplierId.Value;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new GroceryItemUpdated { GroceryItem = this });
        }

        return this;
    }

    public GroceryItem UpdateStock(int quantity, string operation)
    {
        var previousStock = CurrentStock;
        
        switch (operation.ToUpper())
        {
            case "ADD":
                CurrentStock += quantity;
                break;
            case "REMOVE":
                CurrentStock = Math.Max(0, CurrentStock - quantity);
                break;
            case "SET":
                CurrentStock = quantity;
                break;
            default:
                throw new ArgumentException("Invalid stock operation. Use ADD, REMOVE, or SET.", nameof(operation));
        }

        QueueDomainEvent(new GroceryItemStockUpdated 
        { 
            GroceryItem = this, 
            PreviousStock = previousStock, 
            NewStock = CurrentStock,
            Operation = operation,
            Quantity = quantity
        });

        return this;
    }

    public GroceryItem UpdateStockLevels(int? minimumStock, int? maximumStock, int? reorderPoint)
    {
        bool isUpdated = false;

        if (minimumStock.HasValue && MinimumStock != minimumStock.Value)
        {
            MinimumStock = minimumStock.Value;
            isUpdated = true;
        }

        if (maximumStock.HasValue && MaximumStock != maximumStock.Value)
        {
            MaximumStock = maximumStock.Value;
            isUpdated = true;
        }

        if (reorderPoint.HasValue && ReorderPoint != reorderPoint.Value)
        {
            ReorderPoint = reorderPoint.Value;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new GroceryItemStockLevelsUpdated { GroceryItem = this });
        }

        return this;
    }

    public GroceryItem AssignToWarehouseLocation(DefaultIdType? warehouseLocationId)
    {
        if (WarehouseLocationId != warehouseLocationId)
        {
            WarehouseLocationId = warehouseLocationId;
            QueueDomainEvent(new GroceryItemLocationAssigned { GroceryItem = this });
        }

        return this;
    }

    public bool IsLowStock() => CurrentStock <= ReorderPoint;
    public bool IsOutOfStock() => CurrentStock <= 0;
    public bool IsOverStock() => CurrentStock > MaximumStock;
    public bool IsExpiringSoon(int daysThreshold = 7) => 
        IsPerishable && ExpiryDate.HasValue && ExpiryDate.Value <= DateTime.UtcNow.AddDays(daysThreshold);

    public GroceryItem Update(
        string? name,
        string? description,
        string sku,
        string barcode,
        decimal price,
        decimal cost,
        int minimumStock,
        int maximumStock,
        int currentStock,
        int reorderPoint,
        bool isPerishable,
        DateTime? expiryDate,
        string? brand,
        string? manufacturer,
        decimal weight,
        string? weightUnit,
        DefaultIdType? categoryId,
        DefaultIdType? supplierId,
        DefaultIdType? warehouseLocationId)
    {
        bool isUpdated = false;
        bool stockChanged = false;
        var previousStock = CurrentStock;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
            isUpdated = true;
        }

        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
            isUpdated = true;
        }

        if (!string.Equals(SKU, sku, StringComparison.OrdinalIgnoreCase))
        {
            SKU = sku;
            isUpdated = true;
        }

        if (!string.Equals(Barcode, barcode, StringComparison.OrdinalIgnoreCase))
        {
            Barcode = barcode;
            isUpdated = true;
        }

        if (Price != price)
        {
            Price = price;
            isUpdated = true;
        }

        if (Cost != cost)
        {
            Cost = cost;
            isUpdated = true;
        }

        if (MinimumStock != minimumStock)
        {
            MinimumStock = minimumStock;
            isUpdated = true;
        }

        if (MaximumStock != maximumStock)
        {
            MaximumStock = maximumStock;
            isUpdated = true;
        }

        if (CurrentStock != currentStock)
        {
            CurrentStock = currentStock;
            isUpdated = true;
            stockChanged = true;
        }

        if (ReorderPoint != reorderPoint)
        {
            ReorderPoint = reorderPoint;
            isUpdated = true;
        }

        if (IsPerishable != isPerishable)
        {
            IsPerishable = isPerishable;
            isUpdated = true;
        }

        if (ExpiryDate != expiryDate)
        {
            ExpiryDate = expiryDate;
            isUpdated = true;
        }

        if (!string.Equals(Brand, brand, StringComparison.OrdinalIgnoreCase))
        {
            Brand = brand;
            isUpdated = true;
        }

        if (!string.Equals(Manufacturer, manufacturer, StringComparison.OrdinalIgnoreCase))
        {
            Manufacturer = manufacturer;
            isUpdated = true;
        }

        if (Weight != weight)
        {
            Weight = weight;
            isUpdated = true;
        }

        if (!string.Equals(WeightUnit, weightUnit, StringComparison.OrdinalIgnoreCase))
        {
            WeightUnit = weightUnit;
            isUpdated = true;
        }

        if (CategoryId != categoryId)
        {
            CategoryId = categoryId;
            isUpdated = true;
        }

        if (SupplierId != supplierId)
        {
            SupplierId = supplierId;
            isUpdated = true;
        }

        if (WarehouseLocationId != warehouseLocationId)
        {
            WarehouseLocationId = warehouseLocationId;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new GroceryItemUpdated { GroceryItem = this });
        }

        if (stockChanged)
        {
            QueueDomainEvent(new GroceryItemStockUpdated
            {
                GroceryItem = this,
                PreviousStock = previousStock,
                NewStock = CurrentStock,
                Operation = "SET",
                Quantity = CurrentStock - previousStock
            });
        }

        return this;
    }
}
