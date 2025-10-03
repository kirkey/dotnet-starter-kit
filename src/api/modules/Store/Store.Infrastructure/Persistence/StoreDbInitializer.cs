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
                    city: "Metropolis",
                    state: null,
                    country: "Country",
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
                    city: "Metropolis",
                    state: null,
                    country: "Country",
                    postalCode: $"200{i:00}",
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
            for (var i = existingItemSuppliers + 1; i <= 10; i++)
            {
                var item = items[i % items.Count];
                var supplier = suppliers[i % suppliers.Count];
                itemSuppliers.Add(ItemSupplier.Create(
                    itemId: item.Id,
                    supplierId: supplier.Id,
                    supplierPartNumber: $"SUP-PART-{i:000}",
                    unitCost: 5m + i,
                    leadTimeDays: 5 + i,
                    minimumOrderQuantity: 10,
                    packagingQuantity: 12,
                    isPreferred: i % 2 == 0
                ));
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

        // 11) StockAdjustments - TODO: Update to use Items entity instead of GroceryItems
        // Currently commented out pending Items entity implementation

        // 12) CycleCounts - TODO: Update to use Items entity instead of GroceryItems
        // Currently commented out pending Items entity implementation

        // 13) WholesaleContracts - TODO: Implement WholesaleContracts seeding when WholesaleContract and Customer entities are implemented
        // Currently commented out as these entities haven't been created yet

        logger.LogInformation("[{Tenant}] completed seeding store module data", context.TenantInfo!.Identifier);
    }
}
