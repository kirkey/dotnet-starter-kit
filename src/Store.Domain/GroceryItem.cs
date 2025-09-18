namespace Store.Domain;

/// <summary>
/// Represents a product (grocery item) stored and sold by the store.
/// Contains pricing, stock and identification fields.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track stock levels and reorder points.
/// - Provide pricing and weight details for orders.
/// - Support barcode scanning and SKU-based lookups.
/// - Manage perishable item expiry tracking.
/// - Link items to categories and suppliers for organization.
/// - Monitor low stock and overstock conditions.
/// </remarks>
/// <seealso cref="Store.Domain.Events.GroceryItemCreated"/>
/// <seealso cref="Store.Domain.Events.GroceryItemUpdated"/>
/// <seealso cref="Store.Domain.Events.GroceryItemStockUpdated"/>
/// <seealso cref="Store.Domain.Events.GroceryItemStockLevelsUpdated"/>
/// <seealso cref="Store.Domain.Events.GroceryItemLocationAssigned"/>
/// <seealso cref="Store.Domain.Exceptions.GroceryItem.GroceryItemNotFoundException"/>
/// <seealso cref="Store.Domain.Exceptions.GroceryItem.DuplicateGroceryItemSkuException"/>
/// <seealso cref="Store.Domain.Exceptions.GroceryItem.DuplicateGroceryItemBarcodeException"/>
public sealed class GroceryItem : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Stock keeping unit: short unique identifier. Example: "SKU-1234".
    /// Max length: 100.
    /// </summary>
    public string SKU { get; private set; } = default!;

    /// <summary>
    /// Product barcode for scanning. Example: "0123456789012".
    /// Max length: 100.
    /// </summary>
    public string Barcode { get; private set; } = default!;

    /// <summary>
    /// Selling price per unit. Must be &gt;= 0.
    /// Example: 2.49 for $2.49 per item.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// Supplier cost per unit. Must be &gt;= 0.
    /// Example: 1.50 for $1.50 cost per item.
    /// </summary>
    public decimal Cost { get; private set; }

    /// <summary>
    /// Minimum stock to keep (safety stock). Example: 5.
    /// Used to trigger reorder notifications.
    /// </summary>
    public int MinimumStock { get; private set; }

    /// <summary>
    /// Maximum allowed stock (optional). Example: 100.
    /// Used to prevent overordering and storage issues.
    /// </summary>
    public int MaximumStock { get; private set; }

    /// <summary>
    /// Current available stock quantity. Must be &gt;= 0.
    /// Updated by sales, receipts, and adjustments. Default: 0.
    /// </summary>
    public int CurrentStock { get; private set; }

    /// <summary>
    /// Reorder point that triggers procurement. Example: 10.
    /// When CurrentStock &lt;= ReorderPoint, item needs restocking.
    /// </summary>
    public int ReorderPoint { get; private set; }

    /// <summary>
    /// Whether the item is perishable (affects expiry tracking).
    /// Default: false. Set to true for items with expiration dates.
    /// </summary>
    public bool IsPerishable { get; private set; }

    /// <summary>
    /// Optional expiry date when the item is perishable.
    /// Example: 2025-12-31 for items expiring end of year.
    /// </summary>
    public DateTime? ExpiryDate { get; private set; }

    /// <summary>
    /// Brand name (optional). Example: "Dole", "Coca-Cola".
    /// Max length: 200.
    /// </summary>
    public string? Brand { get; private set; }

    /// <summary>
    /// Manufacturer name (optional). Example: "Nestle", "Unilever".
    /// Max length: 200.
    /// </summary>
    public string? Manufacturer { get; private set; }

    /// <summary>
    /// Item weight for shipping/handling. Must be &gt;= 0.
    /// Example: 0.5 for 0.5 kg or 1.2 for 1.2 lbs.
    /// </summary>
    public decimal Weight { get; private set; }

    /// <summary>
    /// Unit of weight measurement. Example: "kg", "lbs", "oz".
    /// Max length: 20.
    /// </summary>
    public string? WeightUnit { get; private set; }

    /// <summary>
    /// Category this item belongs to (optional).
    /// Links to <see cref="Category"/> for organization and reporting.
    /// </summary>
    public DefaultIdType? CategoryId { get; private set; }

    /// <summary>
    /// Supplier this item is purchased from (optional).
    /// Links to <see cref="Supplier"/> for procurement and vendor management.
    /// </summary>
    public DefaultIdType? SupplierId { get; private set; }

    /// <summary>
    /// Warehouse location where item is stored (optional).
    /// Links to <see cref="WarehouseLocation"/> for inventory placement.
    /// </summary>
    public DefaultIdType? WarehouseLocationId { get; private set; }
    
    /// <summary>
    /// Navigation property to the item's category.
    /// </summary>
    public Category Category { get; private set; } = default!;

    /// <summary>
    /// Navigation property to the item's supplier.
    /// </summary>
    public Supplier Supplier { get; private set; } = default!;

    /// <summary>
    /// Navigation property to the item's warehouse location.
    /// </summary>
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

    /// <summary>
    /// Creates a new GroceryItem with the specified details.
    /// </summary>
    /// <remarks>
    /// - Generates a new unique identifier for the GroceryItem.
    /// - SKU and Barcode must be unique across all items.
    /// - Price and Cost must be >= 0.
    /// - MinimumStock, MaximumStock, and CurrentStock must be >= 0.
    /// - ReorderPoint must be >= 0.
    /// - Weight must be >= 0.
    /// - Brand and Manufacturer names are optional.
    /// - ExpiryDate is optional and only relevant if IsPerishable is true.
    /// </remarks>
    /// <param name="name">The name of the grocery item. Required.</param>
    /// <param name="description">A description of the grocery item. Optional.</param>
    /// <param name="sku">The stock keeping unit identifier. Required.</param>
    /// <param name="barcode">The product barcode. Required.</param>
    /// <param name="price">The selling price per unit. Required.</param>
    /// <param name="cost">The supplier cost per unit. Required.</param>
    /// <param name="minimumStock">The minimum stock level to maintain. Required.</param>
    /// <param name="maximumStock">The maximum stock level allowed. Required.</param>
    /// <param name="currentStock">The current stock level. Required.</param>
    /// <param name="reorderPoint">The stock level at which to reorder. Required.</param>
    /// <param name="isPerishable">Indicates if the item is perishable. Required.</param>
    /// <param name="expiryDate">The expiry date if the item is perishable. Optional.</param>
    /// <param name="brand">The brand name. Optional.</param>
    /// <param name="manufacturer">The manufacturer name. Optional.</param>
    /// <param name="weight">The weight of the item. Required.</param>
    /// <param name="weightUnit">The unit of measurement for the weight. Optional.</param>
    /// <param name="categoryId">The category identifier this item belongs to. Optional.</param>
    /// <param name="supplierId">The supplier identifier this item is purchased from. Optional.</param>
    /// <param name="warehouseLocationId">The warehouse location identifier where the item is stored. Optional.</param>
    /// <returns>A new instance of <see cref="GroceryItem"/>.</returns>
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

    /// <summary>
    /// Updates the details of an existing GroceryItem.
    /// </summary>
    /// <remarks>
    /// - Only updates fields that have changed.
    /// - Queues a <see cref="GroceryItemUpdated"/> event if any details are updated.
    /// </remarks>
    /// <param name="name">The new name of the grocery item. Optional.</param>
    /// <param name="description">The new description of the grocery item. Optional.</param>
    /// <param name="price">The new selling price per unit. Optional.</param>
    /// <param name="cost">The new supplier cost per unit. Optional.</param>
    /// <param name="brand">The new brand name. Optional.</param>
    /// <param name="manufacturer">The new manufacturer name. Optional.</param>
    /// <param name="weight">The new weight of the item. Optional.</param>
    /// <param name="weightUnit">The new unit of measurement for the weight. Optional.</param>
    /// <param name="categoryId">The new category identifier this item belongs to. Optional.</param>
    /// <param name="supplierId">The new supplier identifier this item is purchased from. Optional.</param>
    /// <returns>The updated instance of <see cref="GroceryItem"/>.</returns>
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

    /// <summary>
    /// Updates the stock level of the item.
    /// </summary>
    /// <remarks>
    /// - Increments or decrements the stock based on the operation.
    /// - Queues a <see cref="GroceryItemStockUpdated"/> event with the stock change details.
    /// </remarks>
    /// <param name="quantity">The quantity to add, remove, or set.</param>
    /// <param name="operation">The operation to perform: ADD, REMOVE, or SET.</param>
    /// <returns>The updated instance of <see cref="GroceryItem"/>.</returns>
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

    /// <summary>
    /// Updates the stock level parameters: MinimumStock, MaximumStock, and ReorderPoint.
    /// </summary>
    /// <remarks>
    /// - Only updates fields that have changed.
    /// - Queues a <see cref="GroceryItemStockLevelsUpdated"/> event if any stock level parameters are updated.
    /// </remarks>
    /// <param name="minimumStock">The new minimum stock level. Optional.</param>
    /// <param name="maximumStock">The new maximum stock level. Optional.</param>
    /// <param name="reorderPoint">The new reorder point. Optional.</param>
    /// <returns>The updated instance of <see cref="GroceryItem"/>.</returns>
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

    /// <summary>
    /// Assigns the item to a warehouse location.
    /// </summary>
    /// <remarks>
    /// - Updates the WarehouseLocationId property.
    /// - Queues a <see cref="GroceryItemLocationAssigned"/> event if the location is changed.
    /// </remarks>
    /// <param name="warehouseLocationId">The identifier of the warehouse location to assign. Optional.</param>
    /// <returns>The updated instance of <see cref="GroceryItem"/>.</returns>
    public GroceryItem AssignToWarehouseLocation(DefaultIdType? warehouseLocationId)
    {
        if (WarehouseLocationId != warehouseLocationId)
        {
            WarehouseLocationId = warehouseLocationId;
            QueueDomainEvent(new GroceryItemLocationAssigned { GroceryItem = this });
        }

        return this;
    }

    /// <summary>
    /// Checks if the item is low on stock (CurrentStock <= ReorderPoint).
    /// </summary>
    /// <returns>True if the item is low on stock, otherwise false.</returns>
    public bool IsLowStock() => CurrentStock <= ReorderPoint;

    /// <summary>
    /// Checks if the item is out of stock (CurrentStock <= 0).
    /// </summary>
    /// <returns>True if the item is out of stock, otherwise false.</returns>
    public bool IsOutOfStock() => CurrentStock <= 0;

    /// <summary>
    /// Checks if the item is overstocked (CurrentStock > MaximumStock).
    /// </summary>
    /// <returns>True if the item is overstocked, otherwise false.</returns>
    public bool IsOverStock() => CurrentStock > MaximumStock;

    /// <summary>
    /// Checks if the item is expiring soon (within the specified number of days).
    /// </summary>
    /// <param name="daysThreshold">The number of days to check for expiry. Default: 7.</param>
    /// <returns>True if the item is expiring soon, otherwise false.</returns>
    public bool IsExpiringSoon(int daysThreshold = 7) => 
        IsPerishable && ExpiryDate.HasValue && ExpiryDate.Value <= DateTime.UtcNow.AddDays(daysThreshold);

    /// <summary>
    /// Updates multiple properties of the GroceryItem in a single call.
    /// </summary>
    /// <remarks>
    /// - Efficiently updates all fields with a single event queueing.
    /// - SKU and Barcode must remain unique.
    /// - Price, Cost, MinimumStock, MaximumStock, CurrentStock, and ReorderPoint must be >= 0.
    /// - IsPerishable, ExpiryDate, Brand, Manufacturer, Weight, and WeightUnit can be updated.
    /// - CategoryId, SupplierId, and WarehouseLocationId can be reassigned.
    /// </remarks>
    /// <param name="name">The new name of the grocery item. Optional.</param>
    /// <param name="description">The new description of the grocery item. Optional.</param>
    /// <param name="sku">The new stock keeping unit identifier. Required.</param>
    /// <param name="barcode">The new product barcode. Required.</param>
    /// <param name="price">The new selling price per unit. Required.</param>
    /// <param name="cost">The new supplier cost per unit. Required.</param>
    /// <param name="minimumStock">The new minimum stock level to maintain. Required.</param>
    /// <param name="maximumStock">The new maximum stock level allowed. Required.</param>
    /// <param name="currentStock">The new current stock level. Required.</param>
    /// <param name="reorderPoint">The new stock level at which to reorder. Required.</param>
    /// <param name="isPerishable">Indicates if the item is perishable. Required.</param>
    /// <param name="expiryDate">The new expiry date if the item is perishable. Optional.</param>
    /// <param name="brand">The new brand name. Optional.</param>
    /// <param name="manufacturer">The new manufacturer name. Optional.</param>
    /// <param name="weight">The new weight of the item. Required.</param>
    /// <param name="weightUnit">The new unit of measurement for the weight. Optional.</param>
    /// <param name="categoryId">The new category identifier this item belongs to. Optional.</param>
    /// <param name="supplierId">The new supplier identifier this item is purchased from. Optional.</param>
    /// <param name="warehouseLocationId">The new warehouse location identifier where the item is stored. Optional.</param>
    /// <returns>The updated instance of <see cref="GroceryItem"/>.</returns>
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
