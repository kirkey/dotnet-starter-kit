using Store.Domain.Exceptions.Items;

namespace Store.Domain.Entities;

/// <summary>
/// Represents an inventory item with comprehensive stock management, pricing control, and lifecycle tracking.
/// </summary>
/// <remarks>
/// Use cases:
/// - Manage inventory catalog with SKU, barcode, and pricing information for warehouse operations.
/// - Track inventory levels with automated reorder point notifications and stock optimization.
/// - Support barcode scanning for receiving, picking, and inventory management operations.
/// - Handle perishable items with expiration date tracking and automated rotation (FIFO).
/// - Enable multi-location inventory tracking across warehouses and storage locations.
/// - Support supplier relationship management with cost tracking and lead time monitoring.
/// - Facilitate category-based inventory organization and reporting.
/// - Generate inventory reports for purchasing decisions and financial analysis.
/// - Support lot number and serial number tracking for traceability.
/// 
/// Default values and constraints:
/// - SKU: required unique identifier, max 100 characters (example: "ITEM-001", "SKU-12345")
/// - Barcode: required scannable code, max 100 characters (example: "012345678901")
/// - UnitPrice: required standard selling price per unit, must be >= Cost (example: 24.99 for $24.99)
/// - Cost: required supplier cost per unit, must be >= 0 (example: 15.00)
/// - MinimumStock: required safety stock level, >= 0 (example: 10 units minimum)
/// - MaximumStock: required maximum stock capacity, > 0 (example: 500 units maximum)
/// - ReorderPoint: required reorder trigger level, 0..MaximumStock
/// - ReorderQuantity: recommended order quantity when reordering
/// - LeadTimeDays: supplier lead time in days
/// - IsPerishable: false (true for items requiring expiration tracking)
/// - IsSerialTracked: false (true for items requiring serial number tracking)
/// - IsLotTracked: false (true for items requiring lot/batch tracking)
/// - IsActive: true (items are active by default)
/// 
/// Business rules:
/// - SKU must be unique within the system
/// - Barcode must be unique
/// - UnitPrice and Cost must be non-negative and UnitPrice >= Cost
/// - MinimumStock must be less than or equal to MaximumStock
/// - ReorderPoint must be between 0 and MaximumStock
/// - Cannot delete items with transaction history or current stock
/// - Perishable items require proper rotation (FIFO) management
/// - Category and Supplier must exist and be active
/// - Price changes require audit trail for cost analysis
/// - Serial tracked items require unique serial numbers for each unit
/// - Lot tracked items require lot assignment for traceability
/// </remarks>
/// <seealso cref="Store.Domain.Events.ItemCreated"/>
/// <seealso cref="Store.Domain.Events.ItemUpdated"/>
/// <seealso cref="Store.Domain.Events.ItemPriceChanged"/>
/// <seealso cref="Store.Domain.Events.ItemReorderPointReached"/>
/// <seealso cref="Store.Domain.Events.ItemExpiring"/>
/// <seealso cref="ItemNotFoundException"/>
/// <seealso cref="DuplicateItemSkuException"/>
/// <seealso cref="DuplicateItemBarcodeException"/>
/// <seealso cref="InvalidItemStockLevelException"/>
public sealed class Item : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Stock keeping unit: short unique identifier. Example: "ITEM-1234", "SKU-001".
    /// Max length: 100.
    /// </summary>
    public string Sku { get; private set; } = default!;

    /// <summary>
    /// Product barcode for scanning. Example: "0123456789012", "EAN-13-CODE".
    /// Max length: 100.
    /// </summary>
    public string Barcode { get; private set; } = default!;

    /// <summary>
    /// Standard unit price per unit. Must be &gt;= 0.
    /// Example: 24.99 for $24.99 per item.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Supplier cost per unit. Must be &gt;= 0.
    /// Example: 15.00 for $15.00 cost per item.
    /// </summary>
    public decimal Cost { get; private set; }

    /// <summary>
    /// Minimum stock to keep (safety stock). Example: 10.
    /// Used to trigger reorder notifications.
    /// </summary>
    public int MinimumStock { get; private set; }

    /// <summary>
    /// Maximum allowed stock capacity. Example: 500.
    /// Used to prevent overordering and storage issues.
    /// </summary>
    public int MaximumStock { get; private set; }

    /// <summary>
    /// Reorder point that triggers procurement. Example: 25.
    /// When stock &lt;= ReorderPoint, item needs restocking.
    /// </summary>
    public int ReorderPoint { get; private set; }

    /// <summary>
    /// Recommended quantity to order when reordering. Example: 100.
    /// Economic order quantity or standard order size.
    /// </summary>
    public int ReorderQuantity { get; private set; }

    /// <summary>
    /// Supplier lead time in days. Example: 7 for one week.
    /// Used for planning and procurement timing.
    /// </summary>
    public int LeadTimeDays { get; private set; }

    /// <summary>
    /// Whether the item is perishable (affects expiry tracking).
    /// Default: false. Set to true for items with expiration dates.
    /// </summary>
    public bool IsPerishable { get; private set; }

    /// <summary>
    /// Whether the item requires serial number tracking.
    /// Default: false. Set to true for high-value or warranty items.
    /// </summary>
    public bool IsSerialTracked { get; private set; }

    /// <summary>
    /// Whether the item requires lot/batch tracking.
    /// Default: false. Set to true for items needing batch traceability.
    /// </summary>
    public bool IsLotTracked { get; private set; }

    /// <summary>
    /// Shelf life in days for perishable items. Example: 30 for 30-day shelf life.
    /// Used to calculate expiration dates and rotation schedules.
    /// </summary>
    public int? ShelfLifeDays { get; private set; }

    /// <summary>
    /// Brand name (optional). Example: "Acme", "Premium Brand".
    /// Max length: 200.
    /// </summary>
    public string? Brand { get; private set; }

    /// <summary>
    /// Manufacturer name (optional). Example: "ABC Manufacturing", "XYZ Corp".
    /// Max length: 200.
    /// </summary>
    public string? Manufacturer { get; private set; }

    /// <summary>
    /// Manufacturer part number (optional). Example: "MPN-12345".
    /// Max length: 100.
    /// </summary>
    public string? ManufacturerPartNumber { get; private set; }

    /// <summary>
    /// Item weight for shipping/handling. Must be &gt;= 0.
    /// Example: 2.5 for 2.5 kg or 5.5 for 5.5 lbs.
    /// </summary>
    public decimal Weight { get; private set; }

    /// <summary>
    /// Unit of weight measurement. Example: "kg", "lbs", "oz", "g".
    /// Max length: 20.
    /// </summary>
    public string? WeightUnit { get; private set; }

    /// <summary>
    /// Item dimensions: Length. Example: 10.5 for 10.5 inches or cm.
    /// </summary>
    public decimal? Length { get; private set; }

    /// <summary>
    /// Item dimensions: Width. Example: 8.0 for 8.0 inches or cm.
    /// </summary>
    public decimal? Width { get; private set; }

    /// <summary>
    /// Item dimensions: Height. Example: 3.5 for 3.5 inches or cm.
    /// </summary>
    public decimal? Height { get; private set; }

    /// <summary>
    /// Unit of dimension measurement. Example: "in", "cm", "mm".
    /// Max length: 20.
    /// </summary>
    public string? DimensionUnit { get; private set; }

    /// <summary>
    /// Category this item belongs to.
    /// Links to <see cref="Category"/> for organization and reporting.
    /// </summary>
    public DefaultIdType CategoryId { get; private set; }

    /// <summary>
    /// Primary supplier this item is purchased from.
    /// Links to <see cref="Supplier"/> for procurement and vendor management.
    /// </summary>
    public DefaultIdType SupplierId { get; private set; }

    /// <summary>
    /// Unit of measure. Example: "EA" (Each), "BOX", "CASE", "PALLET".
    /// Max length: 20.
    /// </summary>
    public string UnitOfMeasure { get; private set; } = "EA";

    /// <summary>
    /// Navigation property to the item's category.
    /// </summary>
    public Category Category { get; private set; } = default!;

    /// <summary>
    /// Navigation property to the item's primary supplier.
    /// </summary>
    public Supplier Supplier { get; private set; } = default!;

    private Item() { }

    private Item(
        DefaultIdType id,
        string name,
        string? description,
        string sku,
        string barcode,
        decimal unitPrice,
        decimal cost,
        int minimumStock,
        int maximumStock,
        int reorderPoint,
        int reorderQuantity,
        int leadTimeDays,
        bool isPerishable,
        bool isSerialTracked,
        bool isLotTracked,
        int? shelfLifeDays,
        string? brand,
        string? manufacturer,
        string? manufacturerPartNumber,
        decimal weight,
        string? weightUnit,
        decimal? length,
        decimal? width,
        decimal? height,
        string? dimensionUnit,
        DefaultIdType categoryId,
        DefaultIdType supplierId,
        string unitOfMeasure)
    {
        // domain validations
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
        if (name.Length > 200) throw new ArgumentException("Name must not exceed 200 characters", nameof(name));

        if (string.IsNullOrWhiteSpace(sku)) throw new ArgumentException("SKU is required", nameof(sku));
        if (sku.Length > 100) throw new ArgumentException("SKU must not exceed 100 characters", nameof(sku));

        if (string.IsNullOrWhiteSpace(barcode)) throw new ArgumentException("Barcode is required", nameof(barcode));
        if (barcode.Length > 100) throw new ArgumentException("Barcode must not exceed 100 characters", nameof(barcode));

        if (unitPrice < 0m) throw new ArgumentException("UnitPrice must be zero or greater", nameof(unitPrice));
        if (cost < 0m) throw new ArgumentException("Cost must be zero or greater", nameof(cost));
        if (unitPrice < cost) throw new ArgumentException("UnitPrice must be greater than or equal to Cost", nameof(unitPrice));

        if (minimumStock < 0) throw new ArgumentException("MinimumStock must be zero or greater", nameof(minimumStock));
        if (maximumStock <= 0) throw new ArgumentException("MaximumStock must be greater than zero", nameof(maximumStock));
        if (minimumStock > maximumStock) throw new ArgumentException("MinimumStock cannot be greater than MaximumStock", nameof(minimumStock));

        if (reorderPoint < 0) throw new ArgumentException("ReorderPoint must be zero or greater", nameof(reorderPoint));
        if (reorderPoint > maximumStock) throw new ArgumentException("ReorderPoint cannot exceed MaximumStock", nameof(reorderPoint));

        if (reorderQuantity < 0) throw new ArgumentException("ReorderQuantity must be zero or greater", nameof(reorderQuantity));
        if (leadTimeDays < 0) throw new ArgumentException("LeadTimeDays must be zero or greater", nameof(leadTimeDays));

        if (isPerishable && shelfLifeDays is <= 0)
            throw new ArgumentException("ShelfLifeDays must be greater than zero for perishable items", nameof(shelfLifeDays));

        if (weight < 0m) throw new ArgumentException("Weight must be zero or greater", nameof(weight));
        if (weight > 0 && string.IsNullOrWhiteSpace(weightUnit)) throw new ArgumentException("WeightUnit is required when Weight > 0", nameof(weightUnit));
        if (weightUnit is { Length: > 20 }) throw new ArgumentException("WeightUnit must not exceed 20 characters", nameof(weightUnit));

        if (length is < 0m) throw new ArgumentException("Length must be zero or greater", nameof(length));
        if (width is < 0m) throw new ArgumentException("Width must be zero or greater", nameof(width));
        if (height is < 0m) throw new ArgumentException("Height must be zero or greater", nameof(height));
        if ((length.HasValue || width.HasValue || height.HasValue) && string.IsNullOrWhiteSpace(dimensionUnit))
            throw new ArgumentException("DimensionUnit is required when dimensions are specified", nameof(dimensionUnit));
        if (dimensionUnit is { Length: > 20 }) throw new ArgumentException("DimensionUnit must not exceed 20 characters", nameof(dimensionUnit));

        if (brand is { Length: > 200 }) throw new ArgumentException("Brand must not exceed 200 characters", nameof(brand));
        if (manufacturer is { Length: > 200 }) throw new ArgumentException("Manufacturer must not exceed 200 characters", nameof(manufacturer));
        if (manufacturerPartNumber is { Length: > 100 }) throw new ArgumentException("ManufacturerPartNumber must not exceed 100 characters", nameof(manufacturerPartNumber));

        if (categoryId == DefaultIdType.Empty) throw new ArgumentException("CategoryId is required", nameof(categoryId));
        if (supplierId == DefaultIdType.Empty) throw new ArgumentException("SupplierId is required", nameof(supplierId));

        if (string.IsNullOrWhiteSpace(unitOfMeasure)) unitOfMeasure = "EA";
        if (unitOfMeasure.Length > 20) throw new ArgumentException("UnitOfMeasure must not exceed 20 characters", nameof(unitOfMeasure));

        Id = id;
        Name = name;
        Description = description;
        Sku = sku;
        Barcode = barcode;
        UnitPrice = unitPrice;
        Cost = cost;
        MinimumStock = minimumStock;
        MaximumStock = maximumStock;
        ReorderPoint = reorderPoint;
        ReorderQuantity = reorderQuantity;
        LeadTimeDays = leadTimeDays;
        IsPerishable = isPerishable;
        IsSerialTracked = isSerialTracked;
        IsLotTracked = isLotTracked;
        ShelfLifeDays = shelfLifeDays;
        Brand = brand;
        Manufacturer = manufacturer;
        ManufacturerPartNumber = manufacturerPartNumber;
        Weight = weight;
        WeightUnit = weightUnit;
        Length = length;
        Width = width;
        Height = height;
        DimensionUnit = dimensionUnit;
        CategoryId = categoryId;
        SupplierId = supplierId;
        UnitOfMeasure = unitOfMeasure;

        QueueDomainEvent(new ItemCreated { Item = this });
    }

    /// <summary>
    /// Creates a new Item with the specified details.
    /// </summary>
    public static Item Create(
        string name,
        string? description,
        string sku,
        string barcode,
        decimal unitPrice,
        decimal cost,
        int minimumStock,
        int maximumStock,
        int reorderPoint,
        int reorderQuantity,
        int leadTimeDays,
        DefaultIdType categoryId,
        DefaultIdType supplierId,
        string? unitOfMeasure = "EA",
        bool isPerishable = false,
        bool isSerialTracked = false,
        bool isLotTracked = false,
        int? shelfLifeDays = null,
        string? brand = null,
        string? manufacturer = null,
        string? manufacturerPartNumber = null,
        decimal weight = 0,
        string? weightUnit = null,
        decimal? length = null,
        decimal? width = null,
        decimal? height = null,
        string? dimensionUnit = null)
    {
        return new Item(
            DefaultIdType.NewGuid(),
            name,
            description,
            sku,
            barcode,
            unitPrice,
            cost,
            minimumStock,
            maximumStock,
            reorderPoint,
            reorderQuantity,
            leadTimeDays,
            isPerishable,
            isSerialTracked,
            isLotTracked,
            shelfLifeDays,
            brand,
            manufacturer,
            manufacturerPartNumber,
            weight,
            weightUnit,
            length,
            width,
            height,
            dimensionUnit,
            categoryId,
            supplierId,
            unitOfMeasure ?? "EA");
    }

    /// <summary>
    /// Updates the basic details of an existing Item.
    /// </summary>
    public Item Update(
        string? name,
        string? description,
        string? notes,
        string? sku,
        string? barcode,
        decimal? unitPrice,
        decimal? cost,
        DefaultIdType? categoryId,
        DefaultIdType? supplierId,
        string? brand,
        string? manufacturer,
        string? manufacturerPartNumber,
        string? unitOfMeasure)
    {
        bool isUpdated = false;
        decimal oldPrice = UnitPrice;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            if (name.Length > 200) throw new ArgumentException("Name must not exceed 200 characters", nameof(name));
            Name = name;
            isUpdated = true;
        }

        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
            isUpdated = true;
        }

        if (!string.Equals(Notes, notes, StringComparison.OrdinalIgnoreCase))
        {
            if (notes?.Length > 2048) throw new ArgumentException("Notes must not exceed 2048 characters", nameof(notes));
            Notes = notes;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(sku) && !string.Equals(Sku, sku, StringComparison.OrdinalIgnoreCase))
        {
            if (sku.Length > 100) throw new ArgumentException("SKU must not exceed 100 characters", nameof(sku));
            Sku = sku;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(barcode) && !string.Equals(Barcode, barcode, StringComparison.OrdinalIgnoreCase))
        {
            if (barcode.Length > 100) throw new ArgumentException("Barcode must not exceed 100 characters", nameof(barcode));
            Barcode = barcode;
            isUpdated = true;
        }

        if (unitPrice.HasValue && UnitPrice != unitPrice.Value)
        {
            if (unitPrice.Value < 0m) throw new ArgumentException("UnitPrice must be zero or greater", nameof(unitPrice));
            var newCost = cost ?? Cost;
            if (unitPrice.Value < newCost) throw new ArgumentException("UnitPrice must be greater than or equal to Cost", nameof(unitPrice));
            
            UnitPrice = unitPrice.Value;
            isUpdated = true;
            QueueDomainEvent(new ItemPriceChanged { Item = this, OldPrice = oldPrice, NewPrice = UnitPrice });
        }

        if (cost.HasValue && Cost != cost.Value)
        {
            if (cost.Value < 0m) throw new ArgumentException("Cost must be zero or greater", nameof(cost));
            var newPrice = unitPrice ?? UnitPrice;
            if (newPrice < cost.Value) throw new ArgumentException("UnitPrice must be greater than or equal to Cost");
            
            Cost = cost.Value;
            isUpdated = true;
        }

        if (categoryId.HasValue && CategoryId != categoryId.Value)
        {
            if (categoryId.Value == DefaultIdType.Empty) throw new ArgumentException("CategoryId is required", nameof(categoryId));
            CategoryId = categoryId.Value;
            isUpdated = true;
        }

        if (supplierId.HasValue && SupplierId != supplierId.Value)
        {
            if (supplierId.Value == DefaultIdType.Empty) throw new ArgumentException("SupplierId is required", nameof(supplierId));
            SupplierId = supplierId.Value;
            isUpdated = true;
        }

        if (!string.Equals(Brand, brand, StringComparison.OrdinalIgnoreCase))
        {
            if (brand is { Length: > 200 }) throw new ArgumentException("Brand must not exceed 200 characters", nameof(brand));
            Brand = brand;
            isUpdated = true;
        }

        if (!string.Equals(Manufacturer, manufacturer, StringComparison.OrdinalIgnoreCase))
        {
            if (manufacturer is { Length: > 200 }) throw new ArgumentException("Manufacturer must not exceed 200 characters", nameof(manufacturer));
            Manufacturer = manufacturer;
            isUpdated = true;
        }

        if (!string.Equals(ManufacturerPartNumber, manufacturerPartNumber, StringComparison.OrdinalIgnoreCase))
        {
            if (manufacturerPartNumber is { Length: > 100 }) throw new ArgumentException("ManufacturerPartNumber must not exceed 100 characters", nameof(manufacturerPartNumber));
            ManufacturerPartNumber = manufacturerPartNumber;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(unitOfMeasure) && !string.Equals(UnitOfMeasure, unitOfMeasure, StringComparison.OrdinalIgnoreCase))
        {
            if (unitOfMeasure.Length > 20) throw new ArgumentException("UnitOfMeasure must not exceed 20 characters", nameof(unitOfMeasure));
            UnitOfMeasure = unitOfMeasure;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new ItemUpdated { Item = this });
        }

        return this;
    }

    /// <summary>
    /// Updates stock control parameters.
    /// </summary>
    public Item UpdateStockLevels(
        int? minimumStock,
        int? maximumStock,
        int? reorderPoint,
        int? reorderQuantity,
        int? leadTimeDays)
    {
        bool isUpdated = false;

        if (minimumStock.HasValue && MinimumStock != minimumStock.Value)
        {
            if (minimumStock.Value < 0) throw new ArgumentException("MinimumStock must be zero or greater", nameof(minimumStock));
            var newMax = maximumStock ?? MaximumStock;
            if (minimumStock.Value > newMax) throw new ArgumentException("MinimumStock cannot be greater than MaximumStock", nameof(minimumStock));
            
            MinimumStock = minimumStock.Value;
            isUpdated = true;
        }

        if (maximumStock.HasValue && MaximumStock != maximumStock.Value)
        {
            if (maximumStock.Value <= 0) throw new ArgumentException("MaximumStock must be greater than zero", nameof(maximumStock));
            var newMin = minimumStock ?? MinimumStock;
            if (newMin > maximumStock.Value) throw new ArgumentException("MinimumStock cannot be greater than MaximumStock");
            
            MaximumStock = maximumStock.Value;
            isUpdated = true;
        }

        if (reorderPoint.HasValue && ReorderPoint != reorderPoint.Value)
        {
            if (reorderPoint.Value < 0) throw new ArgumentException("ReorderPoint must be zero or greater", nameof(reorderPoint));
            var newMax = maximumStock ?? MaximumStock;
            if (reorderPoint.Value > newMax) throw new ArgumentException("ReorderPoint cannot exceed MaximumStock", nameof(reorderPoint));
            
            ReorderPoint = reorderPoint.Value;
            isUpdated = true;
        }

        if (reorderQuantity.HasValue && ReorderQuantity != reorderQuantity.Value)
        {
            if (reorderQuantity.Value < 0) throw new ArgumentException("ReorderQuantity must be zero or greater", nameof(reorderQuantity));
            ReorderQuantity = reorderQuantity.Value;
            isUpdated = true;
        }

        if (leadTimeDays.HasValue && LeadTimeDays != leadTimeDays.Value)
        {
            if (leadTimeDays.Value < 0) throw new ArgumentException("LeadTimeDays must be zero or greater", nameof(leadTimeDays));
            LeadTimeDays = leadTimeDays.Value;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new ItemUpdated { Item = this });
        }

        return this;
    }

    /// <summary>
    /// Updates physical attributes (dimensions and weight).
    /// </summary>
    public Item UpdatePhysicalAttributes(
        decimal? weight,
        string? weightUnit,
        decimal? length,
        decimal? width,
        decimal? height,
        string? dimensionUnit)
    {
        bool isUpdated = false;

        if (weight.HasValue && Weight != weight.Value)
        {
            if (weight.Value < 0m) throw new ArgumentException("Weight must be zero or greater", nameof(weight));
            if (weight.Value > 0 && string.IsNullOrWhiteSpace(weightUnit) && string.IsNullOrWhiteSpace(WeightUnit))
                throw new ArgumentException("WeightUnit is required when Weight > 0", nameof(weightUnit));
            
            Weight = weight.Value;
            isUpdated = true;
        }

        if (!string.Equals(WeightUnit, weightUnit, StringComparison.OrdinalIgnoreCase))
        {
            if (weightUnit is { Length: > 20 }) throw new ArgumentException("WeightUnit must not exceed 20 characters", nameof(weightUnit));
            WeightUnit = weightUnit;
            isUpdated = true;
        }

        if (length.HasValue && Length != length.Value)
        {
            if (length.Value < 0m) throw new ArgumentException("Length must be zero or greater", nameof(length));
            Length = length.Value;
            isUpdated = true;
        }

        if (width.HasValue && Width != width.Value)
        {
            if (width.Value < 0m) throw new ArgumentException("Width must be zero or greater", nameof(width));
            Width = width.Value;
            isUpdated = true;
        }

        if (height.HasValue && Height != height.Value)
        {
            if (height.Value < 0m) throw new ArgumentException("Height must be zero or greater", nameof(height));
            Height = height.Value;
            isUpdated = true;
        }

        if (!string.Equals(DimensionUnit, dimensionUnit, StringComparison.OrdinalIgnoreCase))
        {
            if (dimensionUnit is { Length: > 20 }) throw new ArgumentException("DimensionUnit must not exceed 20 characters", nameof(dimensionUnit));
            DimensionUnit = dimensionUnit;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new ItemUpdated { Item = this });
        }

        return this;
    }

    /// <summary>
    /// Updates tracking settings for perishable items, serial tracking, and lot tracking.
    /// </summary>
    public Item UpdateTrackingSettings(
        bool? isPerishable,
        bool? isSerialTracked,
        bool? isLotTracked,
        int? shelfLifeDays)
    {
        bool isUpdated = false;

        if (isPerishable.HasValue && IsPerishable != isPerishable.Value)
        {
            IsPerishable = isPerishable.Value;
            isUpdated = true;
        }

        if (isSerialTracked.HasValue && IsSerialTracked != isSerialTracked.Value)
        {
            IsSerialTracked = isSerialTracked.Value;
            isUpdated = true;
        }

        if (isLotTracked.HasValue && IsLotTracked != isLotTracked.Value)
        {
            IsLotTracked = isLotTracked.Value;
            isUpdated = true;
        }

        if (shelfLifeDays.HasValue && ShelfLifeDays != shelfLifeDays.Value)
        {
            var perishable = isPerishable ?? IsPerishable;
            if (perishable && shelfLifeDays.Value <= 0)
                throw new ArgumentException("ShelfLifeDays must be greater than zero for perishable items", nameof(shelfLifeDays));
            
            ShelfLifeDays = shelfLifeDays.Value;
            isUpdated = true;
        }

        // Validate: if item is perishable, ShelfLifeDays should be set
        var finalPerishable = isPerishable ?? IsPerishable;
        var finalShelfLifeDays = shelfLifeDays ?? ShelfLifeDays;
        if (finalPerishable && !finalShelfLifeDays.HasValue)
        {
            throw new ArgumentException("ShelfLifeDays is required when item is perishable");
        }

        if (isUpdated)
        {
            QueueDomainEvent(new ItemUpdated { Item = this });
        }

        return this;
    }
}
