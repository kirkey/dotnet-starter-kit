// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/Store.Infrastructure/Persistence/StoreDbInitializer.cs

using Microsoft.Extensions.Logging;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence;

internal sealed class StoreDbInitializer(
    ILogger<StoreDbInitializer> logger,
    StoreDbContext context) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for store module", context.TenantInfo!.Identifier);
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        // Ensure each domain has at least 10 records. We intentionally check counts and insert only the missing items.

        // 1) Categories
        var existingCategories = await context.Categories.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCategories < 10)
        {
            var categories = new List<Category>();
            var templates = new[]
            {
                ("Uncategorized","", "UNCAT"),
                ("Produce","Fresh fruits and vegetables", "PRODUCE"),
                ("Fruits","Fresh seasonal fruits", "FRUITS"),
                ("Vegetables","Fresh seasonal vegetables", "VEGGIES"),
                ("Organic","Certified organic products", "ORGANIC"),
                ("Dairy & Eggs","Milk, eggs, yogurt, butter", "DAIRYEGGS"),
                ("Milk","Milk and alternatives", "MILK"),
                ("Cheese","Cheese varieties", "CHEESE"),
                ("Yogurt","Yogurt and kefir", "YOGURT"),
                ("Frozen","Frozen foods", "FROZEN"),
                ("Frozen Meals","Frozen prepared meals", "FROMEALS"),
                ("Ice Cream","Ice cream and frozen treats", "ICECREAM"),
                ("Bakery","Bread and baked goods", "BAKERY"),
                ("Breakfast & Cereal","Cereals, oatmeal, pancakes", "BREAKFAST"),
                ("Snacks","Chips, crackers, cookies", "SNACKS"),
                ("Candy","Candy and chocolate", "CANDY"),
                ("Beverages","Drinks and beverages", "BEVERAGES"),
                ("Soft Drinks","Soda and carbonated drinks", "SOFTDRINKS"),
                ("Juice","Fruit and vegetable juices", "JUICE"),
                ("Water","Bottled and sparkling water", "WATER"),
                ("Coffee & Tea","Coffee beans and tea", "COFFEETEA"),
                ("Alcohol","Beer, wine, spirits", "ALCOHOL"),
                ("Beer","Domestic and imported beer", "BEER"),
                ("Wine","Red, white, sparkling wines", "WINE"),
                ("Spirits","Whiskey, vodka, gin, rum", "SPIRITS"),
                ("Meat & Seafood","Fresh meat and seafood", "MEATSEA"),
                ("Beef","Beef cuts", "BEEF"),
                ("Poultry","Chicken and turkey", "POULTRY"),
                ("Pork","Pork cuts", "PORK"),
                ("Seafood","Fish and shellfish", "SEAFOOD"),
                ("Deli","Deli meats and cheeses", "DELI"),
                ("Prepared Foods","Ready-to-eat meals", "PREPFOODS"),
                ("Canned Goods","Canned and jarred foods", "CANNED"),
                ("Pasta & Rice","Pasta, rice, grains", "PASTARICE"),
                ("Baking","Flour, sugar, mixes", "BAKING"),
                ("Condiments","Sauces, ketchup, mustard", "CONDIMENTS"),
                ("Oils & Vinegar","Cooking oils and vinegars", "OILVINEGAR"),
                ("Spices & Seasonings","Herbs, spices, blends", "SPICES"),
                ("Nuts & Dried Fruit","Nuts, seeds, dried fruit", "NUTSDF"),
                ("International","Global cuisine ingredients", "INTERNAT"),
                ("Asian","Asian foods", "ASIAN"),
                ("Latin","Latin foods", "LATIN"),
                ("Indian","Indian foods", "INDIAN"),
                ("Kosher","Kosher certified foods", "KOSHER"),
                ("Gluten-Free","Gluten-free products", "GLUTENFREE"),
                ("Vegan & Plant-Based","Vegan foods", "VEGAN"),
                ("Baby","Baby food and care", "BABY"),
                ("Pet Care","Pet food and supplies", "PETCARE"),
                ("Health & Wellness","Vitamins and supplements", "HEALTH"),
                ("Personal Care","Toiletries and grooming", "PERSONAL"),
                ("Household","Cleaning and household items", "HOUSEHOLD"),
                ("Paper Goods","Paper towels, tissues", "PAPER"),
                ("Laundry","Laundry detergents", "LAUNDRY"),
                ("Kitchen & Dining","Kitchen supplies", "KITCHEN"),
                ("Floral","Flowers and plants", "FLORAL"),
                ("Seasonal","Seasonal items", "SEASONAL")
            };

            for (var i = existingCategories; i < templates.Length; i++)
            {
                var t = templates[i % templates.Length];
                categories.Add(Category.Create(t.Item1 + (i >= templates.Length ? $" {i+1}" : ""), t.Item2, t.Item3 + (i >= templates.Length ? $"{i+1}" : "")));
            }

            await context.Categories.AddRangeAsync(categories, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // 2) Suppliers
        var existingSuppliers = await context.Suppliers.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingSuppliers < 10)
        {
            var suppliers = new List<Supplier>();
            for (var i = existingSuppliers + 1; i <= 10; i++)
            {
                suppliers.Add(Supplier.Create(
                    name: $"Supplier {i}",
                    description: null,
                    code: $"SUP{i:000}",
                    contactPerson: $"Contact {i}",
                    email: $"supplier{i}@example.com",
                    phone: $"+100000000{i:00}",
                    address: $"{i} Supplier St",
                    postalCode: $"100{i:00}",
                    website: null,
                    creditLimit: 10000m + i * 1000m,
                    paymentTermsDays: 30,
                    isActive: true,
                    rating: Math.Min(5m, 1m + i * 0.4m)));
            }
            await context.Suppliers.AddRangeAsync(suppliers, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // 3) Warehouses
        var existingWarehouses = await context.Warehouses.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingWarehouses < 10)
        {
            var warehouses = new List<Warehouse>();
            for (var i = existingWarehouses + 1; i <= 10; i++)
            {
                warehouses.Add(Warehouse.Create(
                    name: $"Warehouse {i}",
                    description: null,
                    code: $"WH{i:00}",
                    address: $"{i} Warehouse Ave",
                    managerName: $"Manager {i}",
                    managerEmail: $"manager{i}@example.com",
                    managerPhone: $"+200000000{i:00}",
                    totalCapacity: 10000m + i * 1000m,
                    capacityUnit: "sqft",
                    warehouseType: "Standard",
                    isActive: true,
                    isMainWarehouse: i == 1));
            }

            await context.Warehouses.AddRangeAsync(warehouses, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // 4) WarehouseLocations
        var existingLocations = await context.WarehouseLocations.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingLocations < 10)
        {
            var warehouses = await context.Warehouses.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var locations = new List<WarehouseLocation>();
            var idx = existingLocations + 1;
            var whIndex = 0;
            while (locations.Count + existingLocations < 10)
            {
                var wh = warehouses[whIndex % warehouses.Count];
                locations.Add(WarehouseLocation.Create(
                    name: $"Location {idx}",
                    description: null,
                    code: $"LOC{idx:000}",
                    aisle: $"A{(idx % 5) + 1}",
                    section: $"S{(idx % 3) + 1}",
                    shelf: $"SH{(idx % 4) + 1}",
                    bin: null,
                    warehouseId: wh.Id,
                    locationType: "Rack",
                    capacity: 1000m + (idx % 5) * 100m,
                    capacityUnit: "units",
                    isActive: true));
                idx++;
                whIndex++;
            }

            await context.WarehouseLocations.AddRangeAsync(locations, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // 5) Items
        var existingItems = await context.Items.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingItems < 10)
        {
            var categories = await context.Categories.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var suppliers = await context.Suppliers.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var items = new List<Item>();
            for (var i = existingItems + 1; i <= 10; i++)
            {
                var category = categories[i % categories.Count];
                var supplier = suppliers[i % suppliers.Count];
                items.Add(Item.Create(
                    name: $"Sample Item {i}",
                    description: $"Sample item description {i}",
                    sku: $"ITEM-{i:000}",
                    barcode: $"0123456789{i:000}",
                    unitPrice: 10m + i,
                    cost: 5m + i,
                    minimumStock: 5,
                    maximumStock: 100,
                    reorderPoint: 10,
                    reorderQuantity: 20,
                    leadTimeDays: 7,
                    isPerishable: i % 2 == 0,
                    isSerialTracked: i % 3 == 0,
                    isLotTracked: i % 4 == 0,
                    shelfLifeDays: i % 2 == 0 ? 30 : null,
                    brand: $"Brand {i}",
                    manufacturer: $"Manufacturer {i}",
                    manufacturerPartNumber: $"MPN-{i:000}",
                    weight: 1.5m + i,
                    weightUnit: "kg",
                    length: 10 + i,
                    width: 5 + i,
                    height: 2 + i,
                    dimensionUnit: "cm",
                    categoryId: category.Id,
                    supplierId: supplier.Id,
                    unitOfMeasure: "EA"
                ));
            }
            await context.Items.AddRangeAsync(items, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // 6) ItemSuppliers
        var existingItemSuppliers = await context.ItemSuppliers.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingItemSuppliers < 10)
        {
            var items = await context.Items.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var suppliers = await context.Suppliers.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var itemSuppliers = new List<ItemSupplier>();
            var uniquePairs = new HashSet<(DefaultIdType, DefaultIdType)>();
            int itemCount = items.Count;
            int supplierCount = suppliers.Count;
            int maxPairs = Math.Min(itemCount * supplierCount, 10);
            int pairIndex = 0;
            for (int itemIdx = 0; itemIdx < itemCount && pairIndex < maxPairs; itemIdx++)
            {
                for (int supplierIdx = 0; supplierIdx < supplierCount && pairIndex < maxPairs; supplierIdx++)
                {
                    var item = items[itemIdx];
                    var supplier = suppliers[supplierIdx];
                    if (uniquePairs.Add((item.Id, supplier.Id)))
                    {
                        itemSuppliers.Add(ItemSupplier.Create(
                            itemId: item.Id,
                            supplierId: supplier.Id,
                            supplierPartNumber: $"SUP-PART-{pairIndex + 1:000}",
                            unitCost: 5m + pairIndex + 1,
                            leadTimeDays: 5 + pairIndex + 1,
                            minimumOrderQuantity: 10,
                            packagingQuantity: 12,
                            isPreferred: pairIndex % 2 == 0
                        ));
                        pairIndex++;
                    }
                }
            }
            await context.ItemSuppliers.AddRangeAsync(itemSuppliers, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // 7) LotNumbers
        var existingLotNumbers = await context.LotNumbers.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingLotNumbers < 10)
        {
            var items = await context.Items.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var suppliers = await context.Suppliers.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var lotNumbers = new List<LotNumber>();
            for (var i = existingLotNumbers + 1; i <= 10; i++)
            {
                var item = items[i % items.Count];
                var supplier = suppliers[i % suppliers.Count];
                var manufactureDate = DateTime.UtcNow.AddDays(-30 - i);
                var expirationDate = item.IsPerishable ? manufactureDate.AddDays(30) : (DateTime?)null;
                lotNumbers.Add(LotNumber.Create(
                    lotCode: $"LOT-2025-{i:000}",
                    itemId: item.Id,
                    supplierId: supplier.Id,
                    manufactureDate: manufactureDate,
                    expirationDate: expirationDate,
                    receiptDate: manufactureDate.AddDays(2),
                    quantityReceived: 100 + i * 5,
                    qualityNotes: i % 2 == 0 ? "Passed QA" : null
                ));
            }
            await context.LotNumbers.AddRangeAsync(lotNumbers, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // 8) SerialNumbers
        var existingSerialNumbers = await context.SerialNumbers.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingSerialNumbers < 10)
        {
            var items = await context.Items.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var warehouses = await context.Warehouses.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var serialNumbers = new List<SerialNumber>();
            for (var i = existingSerialNumbers + 1; i <= 10; i++)
            {
                var item = items[i % items.Count];
                var warehouse = warehouses[i % warehouses.Count];
                var sn = SerialNumber.Create(
                    serialNumberValue: $"SN-{i:000000}",
                    itemId: item.Id,
                    warehouseId: warehouse.Id,
                    warehouseLocationId: null,
                    binId: null,
                    lotNumberId: null,
                    receiptDate: DateTime.UtcNow.AddDays(-i),
                    manufactureDate: DateTime.UtcNow.AddDays(-30 - i),
                    warrantyExpirationDate: DateTime.UtcNow.AddYears(1),
                    externalReference: null,
                    notes: i % 2 == 0 ? "New unit" : null
                );
                if (i % 2 == 0) sn.UpdateStatus("Available");
                else sn.UpdateStatus("Allocated");
                serialNumbers.Add(sn);
            }
            await context.SerialNumbers.AddRangeAsync(serialNumbers, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // 9) InventoryReservations
        var existingReservations = await context.InventoryReservations.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingReservations < 10)
        {
            var items = await context.Items.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var warehouses = await context.Warehouses.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var reservations = new List<InventoryReservation>();
            for (var i = existingReservations + 1; i <= 10; i++)
            {
                var item = items[i % items.Count];
                var warehouse = warehouses[i % warehouses.Count];
                var reservation = InventoryReservation.Create(
                    reservationNumber: $"RES-2025-{i:000}",
                    itemId: item.Id,
                    warehouseId: warehouse.Id,
                    quantityReserved: 10 + i,
                    reservationType: i % 2 == 0 ? "Order" : "Transfer",
                    warehouseLocationId: null,
                    binId: null,
                    lotNumberId: null,
                    referenceNumber: null,
                    expirationDate: DateTime.UtcNow.AddDays(30),
                    reservedBy: $"user{i}"
                );
                // Set status if needed (default is "Active").
                reservations.Add(reservation);
            }
            await context.InventoryReservations.AddRangeAsync(reservations, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // 10) InventoryTransactions
        var existingTransactions = await context.InventoryTransactions.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingTransactions < 10)
        {
            var items = await context.Items.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var warehouses = await context.Warehouses.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var transactions = new List<InventoryTransaction>();
            for (var i = existingTransactions + 1; i <= 10; i++)
            {
                var item = items[i % items.Count];
                var warehouse = warehouses[i % warehouses.Count];
                var quantity = 10 + i;
                var unitCost = 5m + i;
                transactions.Add(InventoryTransaction.Create(
                    transactionNumber: $"TXN-2025-{i:000}",
                    itemId: item.Id,
                    warehouseId: warehouse.Id,
                    warehouseLocationId: null,
                    purchaseOrderId: null,
                    transactionType: i % 2 == 0 ? "IN" : "OUT",
                    reason: i % 2 == 0 ? "Purchase" : "Sale",
                    quantity: quantity,
                    quantityBefore: 100,
                    unitCost: unitCost,
                    transactionDate: DateTime.UtcNow.AddDays(-i),
                    reference: null,
                    notes: null,
                    performedBy: $"user{i}",
                    isApproved: true
                ));
            }
            await context.InventoryTransactions.AddRangeAsync(transactions, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // 11) Bins
        var existingBins = await context.Bins.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingBins < 10)
        {
            var locations = await context.WarehouseLocations.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (locations.Count > 0)
            {
                var bins = new List<Bin>();
                var binTypes = new[] { "Shelf", "Pallet", "Rack", "Floor", "Drawer" };
                for (var i = existingBins + 1; i <= 10; i++)
                {
                    var location = locations[i % locations.Count];
                    bins.Add(Bin.Create(
                        name: $"Bin {i}",
                        description: $"Storage bin {i}",
                        code: $"BIN-{i:000}",
                        warehouseLocationId: location.Id,
                        binType: binTypes[i % binTypes.Length],
                        capacity: 500m + (i * 50m),
                        priority: i % 3
                    ));
                }
                await context.Bins.AddRangeAsync(bins, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        // 12) StockLevels
        var existingStockLevels = await context.StockLevels.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingStockLevels < 10)
        {
            var items = await context.Items.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var warehouses = await context.Warehouses.Take(3).ToListAsync(cancellationToken).ConfigureAwait(false);
            var locations = await context.WarehouseLocations.Take(3).ToListAsync(cancellationToken).ConfigureAwait(false);
            var bins = await context.Bins.Take(3).ToListAsync(cancellationToken).ConfigureAwait(false);
            
            if (items.Count > 0 && warehouses.Count > 0)
            {
                var stockLevels = new List<StockLevel>();
                for (var i = existingStockLevels + 1; i <= 10; i++)
                {
                    var item = items[i % items.Count];
                    var warehouse = warehouses[i % warehouses.Count];
                    var qtyOnHand = 50 + (i * 10);
                    var qtyReserved = i * 2;
                    var qtyAllocated = i;
                    var stockLevel = StockLevel.Create(
                        itemId: item.Id,
                        warehouseId: warehouse.Id,
                        warehouseLocationId: locations.Count > 0 ? locations[i % locations.Count].Id : null,
                        binId: bins.Count > 0 ? bins[i % bins.Count].Id : null,
                        lotNumberId: null,
                        serialNumberId: null,
                        quantityOnHand: qtyOnHand
                    );
                    // Reserve and allocate quantities after creation
                    if (qtyReserved > 0) stockLevel.ReserveQuantity(qtyReserved);
                    if (qtyAllocated > 0) stockLevel.AllocateQuantity(qtyAllocated);
                    stockLevels.Add(stockLevel);
                }
                await context.StockLevels.AddRangeAsync(stockLevels, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        // 13) PurchaseOrders
        var existingPurchaseOrders = await context.PurchaseOrders.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingPurchaseOrders < 10)
        {
            var suppliers = await context.Suppliers.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (suppliers.Count > 0)
            {
                var purchaseOrders = new List<PurchaseOrder>();
                var statuses = new[] { "Draft", "Submitted", "Approved", "Sent", "Received" };
                for (var i = existingPurchaseOrders + 1; i <= 10; i++)
                {
                    var supplier = suppliers[i % suppliers.Count];
                    var orderDate = DateTime.UtcNow.AddDays(-30 + i);
                    purchaseOrders.Add(PurchaseOrder.Create(
                        orderNumber: $"PO-2025-{i:000}",
                        supplierId: supplier.Id,
                        orderDate: orderDate,
                        expectedDeliveryDate: orderDate.AddDays(7 + (i % 5)),
                        status: statuses[i % statuses.Length],
                        notes: i % 2 == 0 ? "Standard order" : "Rush order",
                        deliveryAddress: $"{i} Receiving Dock, Warehouse District",
                        contactPerson: $"Receiving Manager {i}",
                        contactPhone: $"+300000000{i:00}",
                        isUrgent: i % 3 == 0
                    ));
                }
                await context.PurchaseOrders.AddRangeAsync(purchaseOrders, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        // 14) PurchaseOrderItems
        var existingPOItems = await context.PurchaseOrderItems.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingPOItems < 10)
        {
            var purchaseOrders = await context.PurchaseOrders.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var items = await context.Items.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (purchaseOrders.Count > 0 && items.Count > 0)
            {
                var poItems = new List<PurchaseOrderItem>();
                var uniquePairs = new HashSet<(DefaultIdType, DefaultIdType)>();
                
                // Get existing pairs to avoid duplicates
                var existingPairs = await context.PurchaseOrderItems
                    .Select(x => new { x.PurchaseOrderId, x.ItemId })
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
                
                foreach (var pair in existingPairs)
                {
                    uniquePairs.Add((pair.PurchaseOrderId, pair.ItemId));
                }
                
                int itemsNeeded = 10 - existingPOItems;
                int itemCount = 0;
                for (var poIdx = 0; poIdx < purchaseOrders.Count && itemCount < itemsNeeded; poIdx++)
                {
                    for (var itemIdx = 0; itemIdx < items.Count && itemCount < itemsNeeded; itemIdx++)
                    {
                        var po = purchaseOrders[poIdx];
                        var item = items[itemIdx];
                        var pair = (po.Id, item.Id);
                        
                        if (uniquePairs.Add(pair))
                        {
                            var qty = 20 + ((existingPOItems + itemCount + 1) * 5);
                            var unitPrice = 10m + (existingPOItems + itemCount + 1);
                            poItems.Add(PurchaseOrderItem.Create(
                                purchaseOrderId: po.Id,
                                itemId: item.Id,
                                quantity: qty,
                                unitPrice: unitPrice,
                                discountAmount: (existingPOItems + itemCount + 1) % 3 == 0 ? 10m : 0m,
                                notes: (existingPOItems + itemCount + 1) % 2 == 0 ? "Bulk order discount applied" : null
                            ));
                            itemCount++;
                        }
                    }
                }
                
                if (poItems.Count > 0)
                {
                    await context.PurchaseOrderItems.AddRangeAsync(poItems, cancellationToken).ConfigureAwait(false);
                    await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                }
            }
        }

        // 15) GoodsReceipts
        var existingGoodsReceipts = await context.GoodsReceipts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingGoodsReceipts < 10)
        {
            var purchaseOrders = await context.PurchaseOrders.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var warehouses = await context.Warehouses.Take(3).ToListAsync(cancellationToken).ConfigureAwait(false);
            var locations = await context.WarehouseLocations.Take(3).ToListAsync(cancellationToken).ConfigureAwait(false);
            
            if (warehouses.Count > 0)
            {
                var goodsReceipts = new List<GoodsReceipt>();
                for (var i = existingGoodsReceipts + 1; i <= 10; i++)
                {
                    var po = purchaseOrders.Count > 0 ? purchaseOrders[i % purchaseOrders.Count] : null;
                    var warehouse = warehouses[i % warehouses.Count];
                    var location = locations.Count > 0 ? locations[i % locations.Count] : null;
                    
                    var gr = GoodsReceipt.Create(
                        receiptNumber: $"GR-2025-{i:000}",
                        receivedDate: DateTime.UtcNow.AddDays(-15 + i),
                        warehouseId: warehouse.Id,
                        warehouseLocationId: location?.Id,
                        purchaseOrderId: po?.Id,
                        notes: i % 3 == 0 ? "Sample goods receipt" : null
                    );
                    if (i % 2 == 0) gr.MarkReceived();
                    goodsReceipts.Add(gr);
                }
                await context.GoodsReceipts.AddRangeAsync(goodsReceipts, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        // 16) GoodsReceiptItems
        var existingGRItems = await context.GoodsReceiptItems.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingGRItems < 10)
        {
            var goodsReceipts = await context.GoodsReceipts.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var items = await context.Items.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var poItems = await context.PurchaseOrderItems.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            
            if (goodsReceipts.Count > 0 && items.Count > 0)
            {
                var grItems = new List<GoodsReceiptItem>();
                for (var i = existingGRItems + 1; i <= 10; i++)
                {
                    var gr = goodsReceipts[i % goodsReceipts.Count];
                    var item = items[i % items.Count];
                    var poItem = poItems.Count > 0 && i % 2 == 0 ? poItems[i % poItems.Count] : null;
                    
                    grItems.Add(GoodsReceiptItem.Create(
                        receiptId: gr.Id,
                        itemId: item.Id,
                        name: $"Received {item.Name}",
                        quantity: 50 + (i * 5),
                        unitCost: item.Cost + (i * 0.5m),
                        purchaseOrderItemId: poItem?.Id
                    ));
                }
                await context.GoodsReceiptItems.AddRangeAsync(grItems, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        // 17) StockAdjustments
        var existingStockAdjustments = await context.StockAdjustments.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingStockAdjustments < 10)
        {
            var items = await context.Items.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var warehouses = await context.Warehouses.Take(3).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (items.Count > 0 && warehouses.Count > 0)
            {
                var adjustments = new List<StockAdjustment>();
                var adjustmentTypes = new[] { "Increase", "Decrease", "Write-Off", "Found", "Damage" };
                var reasons = new[] { "Physical Count", "Damage", "Expiry", "Found", "Theft", "Quality Issue" };
                for (var i = existingStockAdjustments + 1; i <= 10; i++)
                {
                    var item = items[i % items.Count];
                    var warehouse = warehouses[i % warehouses.Count];
                    var adjType = adjustmentTypes[i % adjustmentTypes.Length];
                    var qtyBefore = 100;
                    var adjQty = 5 + i;
                    var adjustment = StockAdjustment.Create(
                        adjustmentNumber: $"ADJ-2025-{i:000}",
                        itemId: item.Id,
                        warehouseId: warehouse.Id,
                        warehouseLocationId: null,
                        adjustmentDate: DateTime.UtcNow.AddDays(-10 + i),
                        adjustmentType: adjType,
                        reason: reasons[i % reasons.Length],
                        quantityBefore: qtyBefore,
                        adjustmentQuantity: adjQty,
                        unitCost: 5m + i,
                        reference: i % 3 == 0 ? $"REF-{i:000}" : null,
                        notes: i % 4 == 0 ? "High priority adjustment" : null,
                        adjustedBy: $"user{i}"
                    );
                    // Approve some adjustments
                    if (i % 2 == 0) adjustment.Approve($"manager{(i % 3) + 1}");
                    adjustments.Add(adjustment);
                }
                await context.StockAdjustments.AddRangeAsync(adjustments, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        // 18) CycleCounts
        var existingCycleCounts = await context.CycleCounts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCycleCounts < 10)
        {
            var warehouses = await context.Warehouses.Take(3).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (warehouses.Count > 0)
            {
                var cycleCounts = new List<CycleCount>();
                var countTypes = new[] { "Full", "Partial", "ABC", "Random", "Spot" };
                for (var i = existingCycleCounts + 1; i <= 10; i++)
                {
                    var warehouse = warehouses[i % warehouses.Count];
                    var scheduledDate = DateTime.UtcNow.AddDays(-20 + (i * 2));
                    cycleCounts.Add(CycleCount.Create(
                        countNumber: $"CC-2025-{i:000}",
                        warehouseId: warehouse.Id,
                        warehouseLocationId: null,
                        scheduledDate: scheduledDate,
                        countType: countTypes[i % countTypes.Length],
                        counterName: $"Counter {(i % 3) + 1}",
                        supervisorName: $"Supervisor {(i % 2) + 1}",
                        notes: i % 3 == 0 ? "High priority count" : null
                    ));
                }
                await context.CycleCounts.AddRangeAsync(cycleCounts, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        // 19) CycleCountItems
        var existingCCItems = await context.CycleCountItems.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCCItems < 10)
        {
            var cycleCounts = await context.CycleCounts.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var items = await context.Items.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (cycleCounts.Count > 0 && items.Count > 0)
            {
                var ccItems = new List<CycleCountItem>();
                var uniquePairs = new HashSet<(DefaultIdType, DefaultIdType)>();
                
                // Get existing pairs to avoid duplicates
                var existingPairs = await context.CycleCountItems
                    .Select(x => new { x.CycleCountId, x.ItemId })
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
                
                foreach (var pair in existingPairs)
                {
                    uniquePairs.Add((pair.CycleCountId, pair.ItemId));
                }
                
                int itemsNeeded = 10 - existingCCItems;
                int itemCount = 0;
                for (var ccIdx = 0; ccIdx < cycleCounts.Count && itemCount < itemsNeeded; ccIdx++)
                {
                    for (var itemIdx = 0; itemIdx < items.Count && itemCount < itemsNeeded; itemIdx++)
                    {
                        var cc = cycleCounts[ccIdx];
                        var item = items[itemIdx];
                        var pair = (cc.Id, item.Id);
                        
                        if (uniquePairs.Add(pair))
                        {
                            var systemQty = 100 + ((existingCCItems + itemCount + 1) * 10);
                            var countedQty = (existingCCItems + itemCount + 1) % 3 == 0 ? systemQty - 2 : systemQty;
                            ccItems.Add(CycleCountItem.Create(
                                cycleCountId: cc.Id,
                                itemId: item.Id,
                                systemQuantity: systemQty,
                                countedQuantity: (existingCCItems + itemCount + 1) % 2 == 0 ? countedQty : null,
                                notes: (existingCCItems + itemCount + 1) % 3 == 0 ? "Variance detected" : null
                            ));
                            itemCount++;
                        }
                    }
                }
                
                if (ccItems.Count > 0)
                {
                    await context.CycleCountItems.AddRangeAsync(ccItems, cancellationToken).ConfigureAwait(false);
                    await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                }
            }
        }

        // 20) InventoryTransfers
        var existingTransfers = await context.InventoryTransfers.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingTransfers < 10)
        {
            var warehouses = await context.Warehouses.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (warehouses.Count >= 2)
            {
                var transfers = new List<InventoryTransfer>();
                var transportMethods = new[] { "Truck", "Air Freight", "Courier", "Internal Transfer" };
                for (var i = existingTransfers + 1; i <= 10; i++)
                {
                    var fromWh = warehouses[i % warehouses.Count];
                    var toWh = warehouses[(i + 1) % warehouses.Count];
                    if (fromWh.Id == toWh.Id) toWh = warehouses[(i + 2) % warehouses.Count];
                    
                    var transferDate = DateTime.UtcNow.AddDays(-10 + i);
                    transfers.Add(InventoryTransfer.Create(
                        transferNumber: null,
                        unused: null,
                        transferNumberOverride: $"TRF-2025-{i:000}",
                        fromWarehouseId: fromWh.Id,
                        toWarehouseId: toWh.Id,
                        fromLocationId: null,
                        toLocationId: null,
                        transferDate: transferDate,
                        expectedArrivalDate: transferDate.AddDays(2 + (i % 3)),
                        transferType: i % 2 == 0 ? "Store Replenishment" : "Stock Balancing",
                        priority: i % 3 == 0 ? "High" : "Normal",
                        transportMethod: transportMethods[i % transportMethods.Length],
                        notes: i % 4 == 0 ? "Urgent transfer" : null,
                        requestedBy: $"user{i}"
                    ));
                }
                await context.InventoryTransfers.AddRangeAsync(transfers, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        // 21) InventoryTransferItems
        var existingTransferItems = await context.InventoryTransferItems.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingTransferItems < 10)
        {
            var transfers = await context.InventoryTransfers.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var items = await context.Items.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (transfers.Count > 0 && items.Count > 0)
            {
                var transferItems = new List<InventoryTransferItem>();
                var uniquePairs = new HashSet<(DefaultIdType, DefaultIdType)>();
                
                // Get existing pairs to avoid duplicates
                var existingPairs = await context.InventoryTransferItems
                    .Select(x => new { x.InventoryTransferId, x.ItemId })
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
                
                foreach (var pair in existingPairs)
                {
                    uniquePairs.Add((pair.InventoryTransferId, pair.ItemId));
                }
                
                int itemsNeeded = 10 - existingTransferItems;
                int itemCount = 0;
                for (var tIdx = 0; tIdx < transfers.Count && itemCount < itemsNeeded; tIdx++)
                {
                    for (var itemIdx = 0; itemIdx < items.Count && itemCount < itemsNeeded; itemIdx++)
                    {
                        var transfer = transfers[tIdx];
                        var item = items[itemIdx];
                        var pair = (transfer.Id, item.Id);
                        
                        if (uniquePairs.Add(pair))
                        {
                            transferItems.Add(InventoryTransferItem.Create(
                                inventoryTransferId: transfer.Id,
                                itemId: item.Id,
                                quantity: 10 + ((existingTransferItems + itemCount + 1) * 2),
                                unitPrice: 12m + (existingTransferItems + itemCount + 1)
                            ));
                            itemCount++;
                        }
                    }
                }
                
                if (transferItems.Count > 0)
                {
                    await context.InventoryTransferItems.AddRangeAsync(transferItems, cancellationToken).ConfigureAwait(false);
                    await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                }
            }
        }

        // 22) PickLists
        var existingPickLists = await context.PickLists.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingPickLists < 10)
        {
            var warehouses = await context.Warehouses.Take(3).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (warehouses.Count > 0)
            {
                var pickLists = new List<PickList>();
                var pickingTypes = new[] { "Order", "Wave", "Batch", "Zone" };
                for (var i = existingPickLists + 1; i <= 10; i++)
                {
                    var warehouse = warehouses[i % warehouses.Count];
                    pickLists.Add(PickList.Create(
                        pickListNumber: $"PICK-2025-{i:000}",
                        warehouseId: warehouse.Id,
                        pickingType: pickingTypes[i % pickingTypes.Length],
                        priority: i % 3 == 0 ? 10 : 5,
                        referenceNumber: $"ORD-{i:000}",
                        notes: i % 4 == 0 ? "Rush order - high priority" : null
                    ));
                }
                await context.PickLists.AddRangeAsync(pickLists, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        // 23) PickListItems
        var existingPickListItems = await context.PickListItems.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingPickListItems < 10)
        {
            var pickLists = await context.PickLists.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var items = await context.Items.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var bins = await context.Bins.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (pickLists.Count > 0 && items.Count > 0)
            {
                var pickListItems = new List<PickListItem>();
                for (var i = existingPickListItems + 1; i <= 10; i++)
                {
                    var pickList = pickLists[i % pickLists.Count];
                    var item = items[i % items.Count];
                    var qtyToPick = 5 + (i * 2);
                    pickListItems.Add(PickListItem.Create(
                        pickListId: pickList.Id,
                        itemId: item.Id,
                        binId: bins.Count > 0 ? bins[i % bins.Count].Id : null,
                        lotNumberId: null,
                        serialNumberId: null,
                        quantityToPick: qtyToPick,
                        notes: i % 5 == 0 ? "Handle with care" : null
                    ));
                }
                await context.PickListItems.AddRangeAsync(pickListItems, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        // 24) PutAwayTasks
        var existingPutAwayTasks = await context.PutAwayTasks.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingPutAwayTasks < 10)
        {
            var warehouses = await context.Warehouses.Take(3).ToListAsync(cancellationToken).ConfigureAwait(false);
            var goodsReceipts = await context.GoodsReceipts.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (warehouses.Count > 0)
            {
                var putAwayTasks = new List<PutAwayTask>();
                var strategies = new[] { "Standard", "ABC", "CrossDock", "Directed" };
                for (var i = existingPutAwayTasks + 1; i <= 10; i++)
                {
                    var warehouse = warehouses[i % warehouses.Count];
                    var gr = goodsReceipts.Count > 0 ? goodsReceipts[i % goodsReceipts.Count] : null;
                    putAwayTasks.Add(PutAwayTask.Create(
                        taskNumber: $"PUT-2025-{i:000}",
                        warehouseId: warehouse.Id,
                        goodsReceiptId: gr?.Id,
                        priority: i % 3 == 0 ? 10 : 5,
                        putAwayStrategy: strategies[i % strategies.Length],
                        notes: i % 4 == 0 ? "Perishable items - priority put-away" : null
                    ));
                }
                await context.PutAwayTasks.AddRangeAsync(putAwayTasks, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        // 25) PutAwayTaskItems
        var existingPutAwayTaskItems = await context.PutAwayTaskItems.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingPutAwayTaskItems < 10)
        {
            var putAwayTasks = await context.PutAwayTasks.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var items = await context.Items.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var bins = await context.Bins.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (putAwayTasks.Count > 0 && items.Count > 0 && bins.Count > 0)
            {
                var putAwayTaskItems = new List<PutAwayTaskItem>();
                for (var i = existingPutAwayTaskItems + 1; i <= 10; i++)
                {
                    var task = putAwayTasks[i % putAwayTasks.Count];
                    var item = items[i % items.Count];
                    var bin = bins[i % bins.Count];
                    putAwayTaskItems.Add(PutAwayTaskItem.Create(
                        putAwayTaskId: task.Id,
                        itemId: item.Id,
                        toBinId: bin.Id,
                        lotNumberId: null,
                        serialNumberId: null,
                        quantityToPutAway: 20 + (i * 5),
                        notes: i % 5 == 0 ? "Stack carefully" : null
                    ));
                }
                await context.PutAwayTaskItems.AddRangeAsync(putAwayTaskItems, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        // 26) SalesImports
        var existingSalesImports = await context.SalesImports.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingSalesImports < 10)
        {
            var warehouses = await context.Warehouses.Take(3).ToListAsync(cancellationToken).ConfigureAwait(false);
            if (warehouses.Count > 0)
            {
                var salesImports = new List<SalesImport>();
                var statuses = new[] { "COMPLETED", "COMPLETED", "COMPLETED", "FAILED", "PENDING" };
                
                for (var i = existingSalesImports + 1; i <= 10; i++)
                {
                    var warehouse = warehouses[i % warehouses.Count];
                    var importDate = DateTime.UtcNow.AddDays(-i);
                    var salesPeriodFrom = importDate.Date;
                    var salesPeriodTo = salesPeriodFrom.AddDays(1).AddSeconds(-1);
                    var status = statuses[i % statuses.Length];
                    var totalRecordsValue = 10 + (i * 5);
                    int processedRecordsValue;
                    if (status == "FAILED" || status == "PENDING")
                    {
                        processedRecordsValue = 0;
                    }
                    else
                    {
                        processedRecordsValue = totalRecordsValue - (i % 3);
                    }
                    var errorRecordsValue = totalRecordsValue - processedRecordsValue;
                    
                    var salesImport = SalesImport.Create(
                        importNumber: $"IMP-{importDate:yyyyMMdd}-{i:000}",
                        importDate: importDate,
                        salesPeriodFrom: salesPeriodFrom,
                        salesPeriodTo: salesPeriodTo,
                        warehouseId: warehouse.Id,
                        fileName: $"pos_sales_{importDate:yyyyMMdd}.csv",
                        notes: $"Seeded import for testing - Store {warehouse.Name}",
                        processedBy: $"user{i % 3 + 1}"
                    );
                    
                    // Update status and statistics for completed imports
                    if (status != "PENDING")
                    {
                        salesImport.UpdateStatus("PROCESSING");
                        salesImport.UpdateStatistics(
                            totalRecords: totalRecordsValue,
                            processedRecords: processedRecordsValue,
                            errorRecords: errorRecordsValue,
                            totalQuantity: processedRecordsValue * 3,
                            totalValue: processedRecordsValue * 25.50m
                        );
                        salesImport.UpdateStatus(status);
                    }
                    
                    // Add reversal info for some completed imports
                    if (status == "COMPLETED" && i % 5 == 0)
                    {
                        salesImport.Reverse("Incorrect sales data - wrong date", $"user{i % 3 + 1}");
                    }
                    
                    salesImports.Add(salesImport);
                }
                
                await context.SalesImports.AddRangeAsync(salesImports, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        // 27) SalesImportItems
        var existingSalesImportItems = await context.SalesImportItems.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingSalesImportItems < 10)
        {
            var salesImports = await context.SalesImports
                .Where(x => x.Status == "COMPLETED" && !x.IsReversed)
                .Take(3)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            var items = await context.Items.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            var transactions = await context.InventoryTransactions.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            
            if (salesImports.Count > 0 && items.Count > 0)
            {
                var salesImportItems = new List<SalesImportItem>();
                var itemsNeeded = 10 - existingSalesImportItems;
                var itemCount = 0;
                
                for (var siIdx = 0; siIdx < salesImports.Count && itemCount < itemsNeeded; siIdx++)
                {
                    var salesImport = salesImports[siIdx];
                    var itemsPerImport = Math.Min(5, itemsNeeded - itemCount);
                    
                    for (var i = 0; i < itemsPerImport; i++)
                    {
                        var item = items[i % items.Count];
                        var saleDate = salesImport.SalesPeriodFrom.AddHours(8 + i);
                        var quantitySold = 2 + (i * 2);
                        var unitPrice = 5.99m + i;
                        
                        var importItem = SalesImportItem.Create(
                            salesImportId: salesImport.Id,
                            lineNumber: existingSalesImportItems + itemCount + 1,
                            saleDate: saleDate,
                            barcode: item.Barcode,
                            itemName: item.Name,
                            quantitySold: quantitySold,
                            unitPrice: unitPrice
                        );
                        
                        // Mark as processed and link to item and transaction
                        var transaction = transactions.Count > 0 ? transactions[i % transactions.Count] : null;
                        if (transaction != null)
                        {
                            importItem.MarkAsProcessed(item.Id, transaction.Id);
                        }
                        else
                        {
                            // If no transaction, just set the item ID
                            importItem.SetItemId(item.Id);
                        }
                        
                        salesImportItems.Add(importItem);
                        itemCount++;
                    }
                }
                
                if (salesImportItems.Count > 0)
                {
                    await context.SalesImportItems.AddRangeAsync(salesImportItems, cancellationToken).ConfigureAwait(false);
                    await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                }
            }
        }

        logger.LogInformation("[{Tenant}] completed seeding store module data with comprehensive sample data across all {EntityCount} domain entities", 
            context.TenantInfo!.Identifier, 27);
    }
}
