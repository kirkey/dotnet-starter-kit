namespace Store.Domain;

public sealed class GroceryItem : AuditableEntity, IAggregateRoot
{
    public string SKU { get; private set; } = default!;
    public string Barcode { get; private set; } = default!;
    public decimal Price { get; private set; }
    public decimal Cost { get; private set; }
    public int MinimumStock { get; private set; }
    public int MaximumStock { get; private set; }
    public int CurrentStock { get; private set; }
    public int ReorderPoint { get; private set; }
    public bool IsPerishable { get; private set; }
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
