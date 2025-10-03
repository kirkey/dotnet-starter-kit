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

        // 5) GroceryItems
        var existingItems = await context.GroceryItems.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingItems < 10)
        {
            var categories = await context.Categories.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            var suppliers = await context.Suppliers.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            var locations = await context.WarehouseLocations.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);

            var items = new List<GroceryItem>();
            for (var i = existingItems + 1; i <= 10; i++)
            {
                var cat = categories[(i - 1) % categories.Count];
                var sup = suppliers[(i - 1) % suppliers.Count];
                var loc = locations.Count > 0 ? locations[(i - 1) % locations.Count] : null;

                items.Add(GroceryItem.Create(
                    name: $"Item {i}",
                    description: $"Description for item {i}",
                    sku: $"SKU{i:000}",
                    barcode: $"BAR{i:000}",
                    price: 1.5m * i,
                    cost: 1.0m * i,
                    minimumStock: 5,
                    maximumStock: 100,
                    currentStock: 20 + i,
                    reorderPoint: 10,
                    isPerishable: i % 3 == 0,
                    expiryDate: i % 3 == 0 ? DateTime.UtcNow.AddMonths(6) : null,
                    brand: $"Brand{i}",
                    manufacturer: $"Mfg{i}",
                    weight: 0.5m * i,
                    weightUnit: "kg",
                    categoryId: cat.Id,
                    supplierId: sup.Id,
                    warehouseLocationId: loc?.Id));
            }

            await context.GroceryItems.AddRangeAsync(items, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // 6) Customers
        var existingCustomers = await context.Customers.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCustomers < 10)
        {
            var customers = new List<Customer>();
            for (var i = existingCustomers + 1; i <= 10; i++)
            {
                customers.Add(Customer.Create(
                    name: $"Customer {i}",
                    description: null,
                    code: $"CUST{i:000}",
                    customerType: i % 2 == 0 ? "Wholesale" : "Retail",
                    contactPerson: $"Contact {i}",
                    email: $"customer{i}@example.com",
                    phone: $"+300000000{i:00}",
                    address: $"{i} Customer Rd",
                    city: "Metropolis",
                    state: null,
                    country: "Country",
                    postalCode: $"300{i:00}",
                    creditLimit: 5000m + i * 500m,
                    paymentTermsDays: 30,
                    discountPercentage: i % 2 == 0 ? 5m : 0m));
            }
            await context.Customers.AddRangeAsync(customers, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // 7) PriceLists and PriceListItems
        var existingPriceLists = await context.PriceLists.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingPriceLists < 10)
        {
            var priceLists = new List<PriceList>();
            var groceryItems = await context.GroceryItems.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            for (var i = existingPriceLists + 1; i <= 10; i++)
            {
                var pl = PriceList.Create(
                    name: $"PriceList {i}",
                    description: null,
                    priceListName: $"PL{i}",
                    priceListType: i % 2 == 0 ? "Retail" : "Wholesale",
                    effectiveDate: DateTime.UtcNow.AddDays(-30),
                    expiryDate: DateTime.UtcNow.AddYears(1),
                    currency: "USD");

                // add up to 3 items per price list
                foreach (var gi in groceryItems.Take(3))
                {
                    pl.AddItem(gi.Id, gi.Price * 1.1m, discountPercentage: gi.Price > 10 ? 5m : null);
                }

                priceLists.Add(pl);
            }

            await context.PriceLists.AddRangeAsync(priceLists, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // 8) PurchaseOrders and PurchaseOrderItems
        var existingPOs = await context.PurchaseOrders.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingPOs < 10)
        {
            var suppliers = await context.Suppliers.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            var groceryItems = await context.GroceryItems.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            var purchaseOrders = new List<PurchaseOrder>();

            for (var i = existingPOs + 1; i <= 10; i++)
            {
                var sup = suppliers[(i - 1) % suppliers.Count];
                var po = PurchaseOrder.Create(
                    orderNumber: $"PO-{DateTime.UtcNow:yyyyMMddHHmmss}-{i}",
                    supplierId: sup.Id,
                    orderDate: DateTime.UtcNow.AddDays(-i),
                    expectedDeliveryDate: DateTime.UtcNow.AddDays(7 + i));

                // add 2 items
                var gi1 = groceryItems[(i - 1) % groceryItems.Count];
                var gi2 = groceryItems[(i) % groceryItems.Count];
                po.AddItem(gi1.Id, 10 + i, gi1.Cost);
                po.AddItem(gi2.Id, 5 + i, gi2.Cost);
                purchaseOrders.Add(po);
            }

            await context.PurchaseOrders.AddRangeAsync(purchaseOrders, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // 9) SalesOrders and SalesOrderItems
        var existingSOs = await context.SalesOrders.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingSOs < 10)
        {
            var customers = await context.Customers.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            var groceryItems = await context.GroceryItems.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            var salesOrders = new List<SalesOrder>();

            for (var i = existingSOs + 1; i <= 10; i++)
            {
                var cust = customers[(i - 1) % customers.Count];
                var so = SalesOrder.Create(
                    orderNumber: $"SO-{DateTime.UtcNow:yyyyMMddHHmmss}-{i}",
                    customerId: cust.Id,
                    orderDate: DateTime.UtcNow.AddDays(-i),
                    status: "Confirmed",
                    orderType: cust.IsWholesaleCustomer() ? "Wholesale" : "Retail",
                    paymentStatus: "Pending",
                    paymentMethod: "Cash");

                var gi1 = groceryItems[(i - 1) % groceryItems.Count];
                var gi2 = groceryItems[(i) % groceryItems.Count];
                so.AddItem(gi1.Id, 2 + i, gi1.Price);
                so.AddItem(gi2.Id, 1 + i, gi2.Price);
                salesOrders.Add(so);
            }

            await context.SalesOrders.AddRangeAsync(salesOrders, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // 10) InventoryTransfers and Items
        var existingTransfers = await context.InventoryTransfers.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingTransfers < 10)
        {
            var warehouses = await context.Warehouses.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            var groceryItems = await context.GroceryItems.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            var transfers = new List<InventoryTransfer>();

            for (var i = existingTransfers + 1; i <= 10; i++)
            {
                var from = warehouses[(i - 1) % warehouses.Count];
                var to = warehouses[(i) % warehouses.Count];
                if (from.Id == to.Id) continue; // skip invalid same-warehouse transfer

                var tr = InventoryTransfer.Create(null, null, $"TRF-{i}-{DateTime.UtcNow:yyyyMMddHHmmss}", from.Id, to.Id, null, null, DateTime.UtcNow.AddDays(-i), DateTime.UtcNow.AddDays(i), "Standard", "Normal", null, null, $"User{i}");
                var gi = groceryItems[(i - 1) % groceryItems.Count];
                tr.AddItem(gi.Id, 5 + i, gi.Price);
                transfers.Add(tr);
            }

            await context.InventoryTransfers.AddRangeAsync(transfers, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // 11) InventoryTransactions
        var existingTx = await context.InventoryTransactions.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingTx < 10)
        {
            var groceryItems = await context.GroceryItems.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            var transactions = new List<InventoryTransaction>();
            for (var i = existingTx + 1; i <= 10; i++)
            {
                var gi = groceryItems[(i - 1) % groceryItems.Count];
                transactions.Add(InventoryTransaction.Create(
                    transactionNumber: $"ITX-{i}-{DateTime.UtcNow:yyyyMMddHHmmss}",
                    groceryItemId: gi.Id,
                    warehouseId: null,
                    warehouseLocationId: null,
                    purchaseOrderId: null,
                    transactionType: "IN",
                    reason: "Initial stock",
                    quantity: 10 + i,
                    quantityBefore: gi.CurrentStock,
                    unitCost: gi.Cost,
                    transactionDate: DateTime.UtcNow));
            }

            await context.InventoryTransactions.AddRangeAsync(transactions, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // 12) StockAdjustments
        var existingAdjustments = await context.StockAdjustments.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingAdjustments < 10)
        {
            var groceryItems = await context.GroceryItems.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            var adjustments = new List<StockAdjustment>();

            // pick a warehouse to associate with adjustments (warehouseId is required)
            var defaultWarehouse = await context.Warehouses.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            if (defaultWarehouse is null)
            {
                // If no warehouse exists for some reason, create one quickly so seeding can continue
                defaultWarehouse = Warehouse.Create("Default Warehouse", null, "WH-DEF", "1 Default St", "Metropolis", null, "Country", null, "Default Manager", "manager@local", "+1000000000", 10000m);
                await context.Warehouses.AddAsync(defaultWarehouse, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }

            for (var i = existingAdjustments + 1; i <= 10; i++)
            {
                var gi = groceryItems[(i - 1) % groceryItems.Count];
                adjustments.Add(StockAdjustment.Create(
                    adjustmentNumber: $"SA-{i}-{DateTime.UtcNow:yyyyMMddHHmmss}",
                    groceryItemId: gi.Id,
                    warehouseId: defaultWarehouse.Id,
                    warehouseLocationId: null,
                    adjustmentDate: DateTime.UtcNow,
                    adjustmentType: "Physical Count",
                    reason: "Initial adjustment",
                    quantityBefore: gi.CurrentStock,
                    adjustmentQuantity: i,
                    unitCost: gi.Cost));
            }

            await context.StockAdjustments.AddRangeAsync(adjustments, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // 13) CycleCounts and Items
        var existingCounts = await context.CycleCounts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCounts < 10)
        {
            var warehouses = await context.Warehouses.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);
            var groceryItems = await context.GroceryItems.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            var counts = new List<CycleCount>();
            for (var i = existingCounts + 1; i <= 10; i++)
            {
                var wh = warehouses[(i - 1) % warehouses.Count];
                // CycleCount.Create(countNumber, warehouseId, warehouseLocationId, scheduledDate, countType, counterName = null, supervisorName = null, notes = null)
                var cc = CycleCount.Create($"CC-{i}", wh.Id, null, DateTime.UtcNow.AddDays(-i), "Full");
                // add an item
                var gi = groceryItems[(i - 1) % groceryItems.Count];
                cc.AddItem(gi.Id, gi.CurrentStock);
                counts.Add(cc);
            }

            await context.CycleCounts.AddRangeAsync(counts, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        // 14) WholesaleContracts and WholesalePricings
        var existingContracts = await context.WholesaleContracts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingContracts < 10)
        {
            var customers = await context.Customers.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            var groceryItems = await context.GroceryItems.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
            var contracts = new List<WholesaleContract>();
            for (var i = existingContracts + 1; i <= 10; i++)
            {
                var cust = customers[(i - 1) % customers.Count];
                var wc = WholesaleContract.Create(
                    contractNumber: $"WC-{i}",
                    customerId: cust.Id,
                    startDate: DateTime.UtcNow.AddDays(-30),
                    endDate: DateTime.UtcNow.AddMonths(12),
                    minimumOrderValue: 100m * i,
                    volumeDiscountPercentage: Math.Min(50m, i * 2m));

                // add pricing entries
                var gi = groceryItems[(i - 1) % groceryItems.Count];
                wc.WholesalePricings.Add(WholesalePricing.Create(wc.Id, gi.Id, minimumQuantity: 10, maximumQuantity: 100, tierPrice: gi.Price * 0.9m, discountPercentage: 10m, effectiveDate: DateTime.UtcNow));
                contracts.Add(wc);
            }

            await context.WholesaleContracts.AddRangeAsync(contracts, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        logger.LogInformation("[{Tenant}] completed seeding store module data", context.TenantInfo!.Identifier);
    }
}
