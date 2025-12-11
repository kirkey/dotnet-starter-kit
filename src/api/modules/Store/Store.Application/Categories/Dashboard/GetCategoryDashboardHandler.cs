using FSH.Framework.Core.Exceptions;
using FSH.Starter.WebApi.Store.Application.Dashboard;

namespace FSH.Starter.WebApi.Store.Application.Categories.Dashboard;

/// <summary>
/// Query to retrieve comprehensive dashboard data for a specific category.
/// </summary>
public sealed record GetCategoryDashboardQuery(Guid CategoryId) : IRequest<CategoryDashboardResponse>;

public sealed class GetCategoryDashboardHandler(
    IReadRepository<Category> categoryRepository,
    IReadRepository<Item> itemRepository,
    IReadRepository<StockLevel> stockLevelRepository,
    IReadRepository<InventoryTransaction> inventoryTransactionRepository,
    IReadRepository<PurchaseOrderItem> purchaseOrderItemRepository,
    IReadRepository<ItemSupplier> itemSupplierRepository,
    IReadRepository<Supplier> supplierRepository,
    IReadRepository<Warehouse> warehouseRepository,
    ICacheService cacheService,
    ILogger<GetCategoryDashboardHandler> logger)
    : IRequestHandler<GetCategoryDashboardQuery, CategoryDashboardResponse>
{
    public async Task<CategoryDashboardResponse> Handle(GetCategoryDashboardQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"category-dashboard:{request.CategoryId}";

        var cachedResult = await cacheService.GetAsync<CategoryDashboardResponse>(cacheKey, cancellationToken);
        if (cachedResult is not null)
        {
            return cachedResult;
        }

        logger.LogInformation("Generating dashboard for category {CategoryId}", request.CategoryId);

        var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken)
            ?? throw new NotFoundException($"Category {request.CategoryId} not found");

        // Get parent category if exists
        Category? parentCategory = null;
        if (category.ParentCategoryId.HasValue)
        {
            parentCategory = await categoryRepository.GetByIdAsync(category.ParentCategoryId.Value, cancellationToken);
        }

        var today = DateTime.UtcNow.Date;
        var startOfYear = new DateTime(today.Year, 1, 1);
        var startOfMonth = new DateTime(today.Year, today.Month, 1);
        var lastYearStart = startOfYear.AddYears(-1);
        var lastYearEnd = startOfYear;

        // Get all items in this category (including subcategories)
        var allCategoryIds = await GetCategoryWithSubcategoriesAsync(request.CategoryId, cancellationToken);
        var items = await itemRepository.ListAsync(new ItemsByCategoriesSpec(allCategoryIds), cancellationToken);
        var itemIds = items.Select(i => i.Id).ToList();

        // Get stock levels for these items
        var stockLevels = await stockLevelRepository.ListAsync(new StockLevelsByItemsSpec(itemIds), cancellationToken);

        // Get inventory transactions for these items
        var transactions = await inventoryTransactionRepository.ListAsync(new InventoryTransactionsByItemsSpec(itemIds), cancellationToken);

        // Get purchase order items for these items
        var purchaseOrderItems = await purchaseOrderItemRepository.ListAsync(new PurchaseOrderItemsByItemsSpec(itemIds), cancellationToken);

        // Get item-supplier relationships
        var itemSuppliers = await itemSupplierRepository.ListAsync(new ItemSuppliersByItemsSpec(itemIds), cancellationToken);

        // Get subcategories
        var subcategories = await categoryRepository.ListAsync(new SubcategoriesSpec(request.CategoryId), cancellationToken);

        // Get warehouses for distribution
        var warehouses = await warehouseRepository.ListAsync(cancellationToken);
        var warehouseDict = warehouses.ToDictionary(w => w.Id, w => w);

        // Get suppliers for distribution
        var suppliers = await supplierRepository.ListAsync(cancellationToken);
        var supplierDict = suppliers.ToDictionary(s => s.Id, s => s);

        // Calculate metrics
        var itemMetrics = CalculateItemMetrics(items, transactions, startOfMonth);
        var inventoryMetrics = CalculateInventoryMetrics(items, stockLevels, transactions);
        var salesMetrics = CalculateSalesMetrics(transactions, startOfYear, lastYearStart, lastYearEnd);
        var purchaseMetrics = CalculatePurchaseMetrics(purchaseOrderItems, itemSuppliers, startOfYear);
        var subcategorySummary = await CalculateSubcategorySummary(subcategories, cancellationToken);

        // Generate trends
        var salesTrend = GenerateSalesTrend(transactions, 12);
        var inventoryTrend = GenerateInventoryTrend(stockLevels, items, 12);
        var purchaseTrend = GeneratePurchaseTrend(purchaseOrderItems, 12);

        // Top items
        var topItemsBySales = CalculateTopItemsBySales(items, transactions, stockLevels);
        var topItemsByInventory = CalculateTopItemsByInventory(items, stockLevels);

        // Distributions
        var supplierBreakdown = CalculateSupplierDistribution(itemSuppliers, supplierDict);
        var warehouseBreakdown = CalculateWarehouseDistribution(stockLevels, warehouseDict, items);

        // Recent transactions
        var recentTransactions = transactions
            .OrderByDescending(t => t.TransactionDate)
            .Take(10)
            .Select(t =>
            {
                var item = items.FirstOrDefault(i => i.Id == t.ItemId);
                return new CategoryRecentTransaction
                {
                    TransactionId = t.Id,
                    TransactionType = t.TransactionType ?? "Unknown",
                    ItemName = item?.Name ?? "Unknown",
                    Quantity = t.Quantity,
                    Value = t.TotalCost,
                    TransactionDate = t.TransactionDate,
                    WarehouseName = t.WarehouseId.HasValue && warehouseDict.TryGetValue(t.WarehouseId.Value, out var wh) ? wh.Name : null
                };
            }).ToList();

        // Monthly performance
        var monthlyPerformance = GenerateMonthlyPerformance(transactions, purchaseOrderItems, stockLevels, items, 6);

        // Alerts
        var alerts = GenerateAlerts(items, stockLevels);

        var response = new CategoryDashboardResponse
        {
            CategoryId = category.Id,
            CategoryName = category.Name ?? "Unknown",
            Code = category.Code,
            Description = category.Description,
            ImageUrl = category.ImageUrl,
            IsActive = category.IsActive,
            SortOrder = category.SortOrder,
            ParentCategoryId = category.ParentCategoryId,
            ParentCategoryName = parentCategory?.Name,
            Items = itemMetrics,
            Inventory = inventoryMetrics,
            Sales = salesMetrics,
            Purchases = purchaseMetrics,
            Subcategories = subcategorySummary,
            SalesTrend = salesTrend,
            InventoryTrend = inventoryTrend,
            PurchaseTrend = purchaseTrend,
            TopItemsBySales = topItemsBySales,
            TopItemsByInventoryValue = topItemsByInventory,
            SupplierBreakdown = supplierBreakdown,
            WarehouseBreakdown = warehouseBreakdown,
            RecentTransactions = recentTransactions,
            MonthlyPerformance = monthlyPerformance,
            Alerts = alerts
        };

        await cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5), cancellationToken);

        return response;
    }

    private async Task<List<Guid>> GetCategoryWithSubcategoriesAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var result = new List<Guid> { categoryId };
        var subcategories = await categoryRepository.ListAsync(new SubcategoriesSpec(categoryId), cancellationToken);

        foreach (var sub in subcategories)
        {
            var subIds = await GetCategoryWithSubcategoriesAsync(sub.Id, cancellationToken);
            result.AddRange(subIds);
        }

        return result;
    }

    private static ItemMetrics CalculateItemMetrics(
        List<Item> items,
        List<InventoryTransaction> transactions,
        DateTime startOfMonth)
    {
        var newItems = items.Count(i => i.CreatedOn >= startOfMonth);

        var prices = items.Where(i => i.UnitPrice > 0).Select(i => i.UnitPrice).ToList();
        var costs = items.Where(i => i.Cost > 0).Select(i => i.Cost).ToList();

        var avgPrice = prices.Count > 0 ? prices.Average() : 0;
        var avgCost = costs.Count > 0 ? costs.Average() : 0;
        var avgMargin = avgPrice > 0 ? (avgPrice - avgCost) / avgPrice * 100 : 0;

        // Find best selling item
        var salesByItem = transactions
            .Where(t => t.Quantity < 0)
            .GroupBy(t => t.ItemId)
            .Select(g => new { ItemId = g.Key, TotalSold = g.Sum(t => Math.Abs(t.Quantity)) })
            .OrderByDescending(x => x.TotalSold)
            .FirstOrDefault();

        var bestSellingItem = salesByItem != null
            ? items.FirstOrDefault(i => i.Id == salesByItem.ItemId)?.Name
            : null;

        return new ItemMetrics
        {
            TotalItems = items.Count,
            ActiveItems = items.Count, // All items returned are active by default
            InactiveItems = 0,
            NewItemsThisMonth = newItems,
            AverageUnitPrice = avgPrice,
            AverageCost = avgCost,
            AverageMargin = avgMargin,
            HighestPrice = prices.Count > 0 ? prices.Max() : 0,
            LowestPrice = prices.Count > 0 ? prices.Min() : 0,
            BestSellingItem = bestSellingItem
        };
    }

    private static CategoryInventoryMetrics CalculateInventoryMetrics(
        List<Item> items,
        List<StockLevel> stockLevels,
        List<InventoryTransaction> transactions)
    {
        var itemDict = items.ToDictionary(i => i.Id, i => i);
        var totalQty = stockLevels.Sum(s => s.QuantityOnHand);
        var totalReserved = stockLevels.Sum(s => s.QuantityReserved);
        var totalAvailable = totalQty - totalReserved;

        var totalValue = stockLevels.Sum(s =>
        {
            if (itemDict.TryGetValue(s.ItemId, out var item))
            {
                return s.QuantityOnHand * item.UnitPrice;
            }
            return 0;
        });

        var totalCost = stockLevels.Sum(s =>
        {
            if (itemDict.TryGetValue(s.ItemId, out var item))
            {
                return s.QuantityOnHand * item.Cost;
            }
            return 0;
        });

        var itemsWithStock = stockLevels.Select(s => s.ItemId).Distinct().ToList();
        var lowStockItems = stockLevels.Count(s =>
        {
            if (itemDict.TryGetValue(s.ItemId, out var item))
            {
                return s.QuantityOnHand > 0 && s.QuantityOnHand <= item.ReorderPoint;
            }
            return false;
        });
        var outOfStockItems = stockLevels.Count(s => s.QuantityOnHand <= 0);
        var overstockItems = stockLevels.Count(s =>
        {
            if (itemDict.TryGetValue(s.ItemId, out var item))
            {
                return s.QuantityOnHand > item.MaximumStock;
            }
            return false;
        });

        // Calculate turnover
        var salesQty = transactions.Where(t => t.Quantity < 0).Sum(t => Math.Abs(t.Quantity));
        var avgInventory = totalQty > 0 ? totalQty : 1;
        var turnover = avgInventory > 0 ? (decimal)salesQty / avgInventory : 0;

        return new CategoryInventoryMetrics
        {
            TotalQuantityOnHand = totalQty,
            TotalQuantityAvailable = totalAvailable,
            TotalQuantityReserved = totalReserved,
            TotalInventoryValue = totalValue,
            TotalInventoryCost = totalCost,
            ItemsInStock = itemsWithStock.Count(id => stockLevels.Any(s => s.ItemId == id && s.QuantityOnHand > 0)),
            ItemsLowStock = lowStockItems,
            ItemsOutOfStock = outOfStockItems,
            ItemsOverstock = overstockItems,
            InventoryTurnover = turnover,
            AverageDaysOfSupply = turnover > 0 ? (int)(365 / (double)turnover) : 0
        };
    }

    private static CategorySalesMetrics CalculateSalesMetrics(
        List<InventoryTransaction> transactions,
        DateTime startOfYear,
        DateTime lastYearStart,
        DateTime lastYearEnd)
    {
        var salesTransactions = transactions.Where(t => t.Quantity < 0).ToList();
        var salesYTD = salesTransactions.Where(t => t.TransactionDate >= startOfYear).ToList();
        var salesLastYear = salesTransactions.Where(t => t.TransactionDate >= lastYearStart && t.TransactionDate < lastYearEnd).ToList();

        var totalValue = salesTransactions.Sum(t => Math.Abs(t.TotalCost));
        var totalValueYTD = salesYTD.Sum(t => Math.Abs(t.TotalCost));
        var totalValueLastYear = salesLastYear.Sum(t => Math.Abs(t.TotalCost));

        var totalUnits = salesTransactions.Sum(t => Math.Abs(t.Quantity));
        var totalUnitsYTD = salesYTD.Sum(t => Math.Abs(t.Quantity));

        var growth = totalValueLastYear > 0
            ? (totalValueYTD - totalValueLastYear) / totalValueLastYear * 100
            : 0;

        var avgSaleValue = salesTransactions.Count > 0 ? totalValue / salesTransactions.Count : 0;

        return new CategorySalesMetrics
        {
            TotalSalesValue = totalValue,
            TotalSalesValueYTD = totalValueYTD,
            TotalSalesValueLastYear = totalValueLastYear,
            TotalUnitsSold = totalUnits,
            TotalUnitsSoldYTD = totalUnitsYTD,
            TransactionCount = salesTransactions.Count,
            AverageSaleValue = avgSaleValue,
            SalesGrowthPercentage = growth,
            MarginPercentage = 0
        };
    }

    private static CategoryPurchaseMetrics CalculatePurchaseMetrics(
        List<PurchaseOrderItem> purchaseOrderItems,
        List<ItemSupplier> itemSuppliers,
        DateTime startOfYear)
    {
        var ytdItems = purchaseOrderItems.Where(p => p.CreatedOn >= startOfYear).ToList();

        var totalValue = purchaseOrderItems.Sum(p => p.TotalPrice);
        var totalValueYTD = ytdItems.Sum(p => p.TotalPrice);

        var totalUnits = purchaseOrderItems.Sum(p => p.Quantity);
        var totalUnitsYTD = ytdItems.Sum(p => p.Quantity);

        var uniqueSuppliers = itemSuppliers.Select(s => s.SupplierId).Distinct().Count();
        var avgLeadTime = itemSuppliers.Where(s => s.LeadTimeDays > 0).Select(s => s.LeadTimeDays).DefaultIfEmpty(0).Average();

        return new CategoryPurchaseMetrics
        {
            TotalPurchaseValue = totalValue,
            TotalPurchaseValueYTD = totalValueYTD,
            TotalUnitsReceived = totalUnits,
            TotalUnitsReceivedYTD = totalUnitsYTD,
            PurchaseOrderCount = purchaseOrderItems.Select(p => p.PurchaseOrderId).Distinct().Count(),
            AveragePurchaseValue = purchaseOrderItems.Count > 0 ? totalValue / purchaseOrderItems.Count : 0,
            UniqueSuppliers = uniqueSuppliers,
            AverageLeadTimeDays = (decimal)avgLeadTime
        };
    }

    private async Task<SubcategorySummary> CalculateSubcategorySummary(
        List<Category> subcategories,
        CancellationToken cancellationToken)
    {
        var activeSubcategories = subcategories.Where(c => c.IsActive).ToList();

        var topSubcategories = new List<SubcategoryInfo>();
        foreach (var sub in activeSubcategories.Take(5))
        {
            var subItems = await itemRepository.ListAsync(new ItemsByCategorySpec(sub.Id), cancellationToken);
            var subItemIds = subItems.Select(i => i.Id).ToList();
            var subStockLevels = await stockLevelRepository.ListAsync(new StockLevelsByItemsSpec(subItemIds), cancellationToken);

            var inventoryValue = subStockLevels.Sum(s =>
            {
                var item = subItems.FirstOrDefault(i => i.Id == s.ItemId);
                return item != null ? s.QuantityOnHand * item.UnitPrice : 0;
            });

            topSubcategories.Add(new SubcategoryInfo
            {
                CategoryId = sub.Id,
                CategoryName = sub.Name ?? "Unknown",
                ItemCount = subItems.Count,
                InventoryValue = inventoryValue,
                SalesValue = 0
            });
        }

        return new SubcategorySummary
        {
            TotalSubcategories = subcategories.Count,
            ActiveSubcategories = activeSubcategories.Count,
            TopSubcategories = topSubcategories.OrderByDescending(s => s.InventoryValue).ToList()
        };
    }

    private static List<TimeSeriesDataPoint> GenerateSalesTrend(List<InventoryTransaction> transactions, int months)
    {
        var result = new List<TimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);
            var monthSales = transactions
                .Where(t => t.Quantity < 0 && t.TransactionDate >= monthStart && t.TransactionDate < monthEnd)
                .Sum(t => Math.Abs(t.TotalCost));

            result.Add(new TimeSeriesDataPoint
            {
                Date = monthStart,
                Value = monthSales,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<TimeSeriesDataPoint> GenerateInventoryTrend(
        List<StockLevel> stockLevels,
        List<Item> items,
        int months)
    {
        var result = new List<TimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;
        var itemDict = items.ToDictionary(i => i.Id, i => i);

        // Current inventory value
        var currentValue = stockLevels.Sum(s =>
        {
            if (itemDict.TryGetValue(s.ItemId, out var item))
            {
                return s.QuantityOnHand * item.UnitPrice;
            }
            return 0;
        });

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            result.Add(new TimeSeriesDataPoint
            {
                Date = monthStart,
                Value = currentValue,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<TimeSeriesDataPoint> GeneratePurchaseTrend(List<PurchaseOrderItem> items, int months)
    {
        var result = new List<TimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);
            var monthValue = items
                .Where(p => p.CreatedOn >= monthStart && p.CreatedOn < monthEnd)
                .Sum(p => p.TotalPrice);

            result.Add(new TimeSeriesDataPoint
            {
                Date = monthStart,
                Value = monthValue,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<TopCategoryItem> CalculateTopItemsBySales(
        List<Item> items,
        List<InventoryTransaction> transactions,
        List<StockLevel> stockLevels)
    {
        var salesByItem = transactions
            .Where(t => t.Quantity < 0)
            .GroupBy(t => t.ItemId)
            .Select(g => new
            {
                ItemId = g.Key,
                TotalSales = g.Sum(t => Math.Abs(t.TotalCost)),
                UnitsSold = g.Sum(t => Math.Abs(t.Quantity))
            })
            .OrderByDescending(x => x.TotalSales)
            .Take(10)
            .ToList();

        return salesByItem.Select(s =>
        {
            var item = items.FirstOrDefault(i => i.Id == s.ItemId);
            var stock = stockLevels.Where(sl => sl.ItemId == s.ItemId).Sum(sl => sl.QuantityOnHand);
            var stockStatus = stock <= 0 ? "Out of Stock"
                : (item != null && stock <= item.ReorderPoint) ? "Low Stock"
                : (item != null && stock > item.MaximumStock) ? "Overstock"
                : "Healthy";

            return new TopCategoryItem
            {
                ItemId = s.ItemId,
                ItemName = item?.Name ?? "Unknown",
                Sku = item?.Sku,
                ImageUrl = item?.ImageUrl,
                UnitPrice = item?.UnitPrice ?? 0,
                TotalSales = s.TotalSales,
                UnitsSold = s.UnitsSold,
                QuantityOnHand = stock,
                InventoryValue = stock * (item?.UnitPrice ?? 0),
                StockStatus = stockStatus
            };
        }).ToList();
    }

    private static List<TopCategoryItem> CalculateTopItemsByInventory(
        List<Item> items,
        List<StockLevel> stockLevels)
    {
        var inventoryByItem = stockLevels
            .GroupBy(s => s.ItemId)
            .Select(g =>
            {
                var item = items.FirstOrDefault(i => i.Id == g.Key);
                var qty = g.Sum(s => s.QuantityOnHand);
                return new
                {
                    ItemId = g.Key,
                    Item = item,
                    Quantity = qty,
                    Value = qty * (item?.UnitPrice ?? 0)
                };
            })
            .OrderByDescending(x => x.Value)
            .Take(10)
            .ToList();

        return inventoryByItem.Select(i =>
        {
            var stockStatus = i.Quantity <= 0 ? "Out of Stock"
                : (i.Item != null && i.Quantity <= i.Item.ReorderPoint) ? "Low Stock"
                : (i.Item != null && i.Quantity > i.Item.MaximumStock) ? "Overstock"
                : "Healthy";

            return new TopCategoryItem
            {
                ItemId = i.ItemId,
                ItemName = i.Item?.Name ?? "Unknown",
                Sku = i.Item?.Sku,
                ImageUrl = i.Item?.ImageUrl,
                UnitPrice = i.Item?.UnitPrice ?? 0,
                TotalSales = 0,
                UnitsSold = 0,
                QuantityOnHand = i.Quantity,
                InventoryValue = i.Value,
                StockStatus = stockStatus
            };
        }).ToList();
    }

    private static List<SupplierDistribution> CalculateSupplierDistribution(
        List<ItemSupplier> itemSuppliers,
        Dictionary<Guid, Supplier> supplierDict)
    {
        return itemSuppliers
            .GroupBy(s => s.SupplierId)
            .Where(g => supplierDict.ContainsKey(g.Key))
            .Select(g => new SupplierDistribution
            {
                SupplierId = g.Key,
                SupplierName = supplierDict[g.Key].Name ?? "Unknown",
                ItemCount = g.Count(),
                TotalPurchaseValue = 0,
                Percentage = 0
            })
            .OrderByDescending(s => s.ItemCount)
            .Take(10)
            .ToList();
    }

    private static List<WarehouseDistribution> CalculateWarehouseDistribution(
        List<StockLevel> stockLevels,
        Dictionary<Guid, Warehouse> warehouseDict,
        List<Item> items)
    {
        var itemDict = items.ToDictionary(i => i.Id, i => i);
        var totalValue = stockLevels.Sum(s =>
        {
            if (itemDict.TryGetValue(s.ItemId, out var item))
            {
                return s.QuantityOnHand * item.UnitPrice;
            }
            return 0;
        });

        return stockLevels
            .GroupBy(s => s.WarehouseId)
            .Where(g => warehouseDict.ContainsKey(g.Key))
            .Select(g =>
            {
                var qty = g.Sum(s => s.QuantityOnHand);
                var value = g.Sum(s =>
                {
                    if (itemDict.TryGetValue(s.ItemId, out var item))
                    {
                        return s.QuantityOnHand * item.UnitPrice;
                    }
                    return 0;
                });

                return new WarehouseDistribution
                {
                    WarehouseId = g.Key,
                    WarehouseName = warehouseDict[g.Key].Name ?? "Unknown",
                    QuantityOnHand = qty,
                    InventoryValue = value,
                    Percentage = totalValue > 0 ? value / totalValue * 100 : 0
                };
            })
            .OrderByDescending(w => w.InventoryValue)
            .Take(10)
            .ToList();
    }

    private static List<CategoryMonthlyComparison> GenerateMonthlyPerformance(
        List<InventoryTransaction> transactions,
        List<PurchaseOrderItem> purchaseOrderItems,
        List<StockLevel> stockLevels,
        List<Item> items,
        int months)
    {
        var result = new List<CategoryMonthlyComparison>();
        var today = DateTime.UtcNow.Date;
        var itemDict = items.ToDictionary(i => i.Id, i => i);

        var currentInventoryValue = stockLevels.Sum(s =>
        {
            if (itemDict.TryGetValue(s.ItemId, out var item))
            {
                return s.QuantityOnHand * item.UnitPrice;
            }
            return 0;
        });

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);

            var monthSales = transactions
                .Where(t => t.Quantity < 0 && t.TransactionDate >= monthStart && t.TransactionDate < monthEnd)
                .ToList();

            var monthPurchases = purchaseOrderItems
                .Where(p => p.CreatedOn >= monthStart && p.CreatedOn < monthEnd)
                .ToList();

            result.Add(new CategoryMonthlyComparison
            {
                Month = monthStart.ToString("MMMM"),
                Year = monthStart.Year,
                SalesValue = monthSales.Sum(t => Math.Abs(t.TotalCost)),
                UnitsSold = monthSales.Sum(t => Math.Abs(t.Quantity)),
                PurchaseValue = monthPurchases.Sum(p => p.TotalPrice),
                UnitsReceived = monthPurchases.Sum(p => p.Quantity),
                InventoryValue = currentInventoryValue,
                MarginPercentage = 0
            });
        }

        return result;
    }

    private static List<CategoryAlert> GenerateAlerts(List<Item> items, List<StockLevel> stockLevels)
    {
        var alerts = new List<CategoryAlert>();
        var today = DateTime.UtcNow;
        var itemDict = items.ToDictionary(i => i.Id, i => i);

        // Low stock alerts
        var lowStockItems = stockLevels
            .Where(s => itemDict.ContainsKey(s.ItemId))
            .Where(s => s.QuantityOnHand > 0 && s.QuantityOnHand <= itemDict[s.ItemId].ReorderPoint)
            .Take(5)
            .Select(s => new CategoryAlert
            {
                AlertType = "Low Stock",
                Severity = "Warning",
                Message = $"Low stock for {itemDict[s.ItemId].Name}: {s.QuantityOnHand} units",
                CreatedDate = today,
                RelatedItemId = s.ItemId,
                RelatedItemName = itemDict[s.ItemId].Name
            });
        alerts.AddRange(lowStockItems);

        // Out of stock alerts
        var outOfStockItems = stockLevels
            .Where(s => itemDict.ContainsKey(s.ItemId))
            .Where(s => s.QuantityOnHand <= 0)
            .Take(5)
            .Select(s => new CategoryAlert
            {
                AlertType = "Out of Stock",
                Severity = "Critical",
                Message = $"Out of stock: {itemDict[s.ItemId].Name}",
                CreatedDate = today,
                RelatedItemId = s.ItemId,
                RelatedItemName = itemDict[s.ItemId].Name
            });
        alerts.AddRange(outOfStockItems);

        return alerts.Take(10).ToList();
    }
}

// Specification classes for category dashboard
public sealed class ItemsByCategoriesSpec : Specification<Item>
{
    public ItemsByCategoriesSpec(List<Guid> categoryIds)
    {
        Query.Where(i => categoryIds.Contains(i.CategoryId));
    }
}

public sealed class ItemsByCategorySpec : Specification<Item>
{
    public ItemsByCategorySpec(Guid categoryId)
    {
        Query.Where(i => i.CategoryId == categoryId);
    }
}

public sealed class StockLevelsByItemsSpec : Specification<StockLevel>
{
    public StockLevelsByItemsSpec(List<Guid> itemIds)
    {
        Query.Where(s => itemIds.Contains(s.ItemId));
    }
}

public sealed class InventoryTransactionsByItemsSpec : Specification<InventoryTransaction>
{
    public InventoryTransactionsByItemsSpec(List<Guid> itemIds)
    {
        Query.Where(t => itemIds.Contains(t.ItemId))
            .OrderByDescending(t => t.TransactionDate);
    }
}

public sealed class PurchaseOrderItemsByItemsSpec : Specification<PurchaseOrderItem>
{
    public PurchaseOrderItemsByItemsSpec(List<Guid> itemIds)
    {
        Query.Where(p => itemIds.Contains(p.ItemId))
            .Include(p => p.Item);
    }
}

public sealed class ItemSuppliersByItemsSpec : Specification<ItemSupplier>
{
    public ItemSuppliersByItemsSpec(List<Guid> itemIds)
    {
        Query.Where(s => itemIds.Contains(s.ItemId));
    }
}

public sealed class SubcategoriesSpec : Specification<Category>
{
    public SubcategoriesSpec(Guid parentCategoryId)
    {
        Query.Where(c => c.ParentCategoryId == parentCategoryId);
    }
}
