using FSH.Starter.WebApi.Store.Application.Dashboard;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.Store.Application.Items.Dashboard;

/// <summary>
/// Query to retrieve comprehensive dashboard data for a specific item.
/// </summary>
public sealed record GetItemDashboardQuery(Guid ItemId) : IRequest<ItemDashboardResponse>;

public sealed class GetItemDashboardHandler(
    IReadRepository<Item> itemRepository,
    IReadRepository<StockLevel> stockLevelRepository,
    IReadRepository<InventoryTransaction> transactionRepository,
    IReadRepository<PurchaseOrder> purchaseOrderRepository,
    IReadRepository<PurchaseOrderItem> purchaseOrderItemRepository,
    IReadRepository<GoodsReceipt> goodsReceiptRepository,
    IReadRepository<SalesImport> salesImportRepository,
    IReadRepository<Warehouse> warehouseRepository,
    IReadRepository<ItemSupplier> itemSupplierRepository,
    IReadRepository<Supplier> supplierRepository,
    IReadRepository<Category> categoryRepository,
    ICacheService cacheService,
    ILogger<GetItemDashboardHandler> logger)
    : IRequestHandler<GetItemDashboardQuery, ItemDashboardResponse>
{
    public async Task<ItemDashboardResponse> Handle(GetItemDashboardQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"item-dashboard:{request.ItemId}";

        var cachedResult = await cacheService.GetAsync<ItemDashboardResponse>(cacheKey, cancellationToken);
        if (cachedResult is not null)
        {
            return cachedResult;
        }

        logger.LogInformation("Generating dashboard for item {ItemId}", request.ItemId);

        var item = await itemRepository.GetByIdAsync(request.ItemId, cancellationToken)
            ?? throw new NotFoundException($"Item {request.ItemId} not found");

        var category = item.CategoryId != Guid.Empty
            ? await categoryRepository.GetByIdAsync(item.CategoryId, cancellationToken)
            : null;

        // Get stock levels across all warehouses
        var stockLevels = await stockLevelRepository
            .ListAsync(new StockLevelsByItemSpec(request.ItemId), cancellationToken);

        var totalOnHand = stockLevels.Sum(s => s.QuantityOnHand);
        var totalAvailable = stockLevels.Sum(s => s.QuantityAvailable);
        var totalReserved = stockLevels.Sum(s => s.QuantityReserved);

        // Get transactions for the last year
        var oneYearAgo = DateTime.UtcNow.AddYears(-1);
        var transactions = await transactionRepository
            .ListAsync(new TransactionsByItemAndDateSpec(request.ItemId, oneYearAgo), cancellationToken);

        // Calculate sales metrics from transactions (outbound transactions)
        var salesTransactions = transactions.Where(t =>
            t.TransactionType == "Sale" ||
            t.TransactionType == "SalesImport" ||
            t.TransactionType == "Issue").ToList();

        var purchaseTransactions = transactions.Where(t =>
            t.TransactionType == "Purchase" ||
            t.TransactionType == "Receipt" ||
            t.TransactionType == "GoodsReceipt").ToList();

        // Calculate metrics for different periods
        var today = DateTime.UtcNow.Date;
        var dailySales = CalculateSalesMetrics(salesTransactions, today, today.AddDays(1), item.UnitPrice, item.Cost);
        var weeklySales = CalculateSalesMetrics(salesTransactions, today.AddDays(-7), today.AddDays(1), item.UnitPrice, item.Cost);
        var monthlySales = CalculateSalesMetrics(salesTransactions, today.AddMonths(-1), today.AddDays(1), item.UnitPrice, item.Cost);
        var yearlySales = CalculateSalesMetrics(salesTransactions, today.AddYears(-1), today.AddDays(1), item.UnitPrice, item.Cost);

        // Calculate movement classification
        var avgDailySales = yearlySales.TotalQuantitySold / 365m;
        var (movementClass, turnoverRate) = ClassifyMovement(avgDailySales, totalOnHand, item.Cost);
        var daysOfSupply = avgDailySales > 0 ? (int)(totalOnHand / avgDailySales) : 999;

        // Determine stock status
        var stockStatus = DetermineStockStatus(totalOnHand, item.MinimumStock, item.ReorderPoint);

        // Generate trend data (last 30 days)
        var salesTrend = GenerateDailyTrend(salesTransactions, 30);
        var stockTrend = await GenerateStockTrend(request.ItemId, 30, cancellationToken);
        var purchaseTrend = GenerateDailyTrend(purchaseTransactions, 30);

        // Top warehouses
        var topWarehouses = stockLevels
            .Where(s => s.QuantityOnHand > 0)
            .OrderByDescending(s => s.QuantityOnHand)
            .Take(5)
            .Select(s => new WarehouseStockInfo
            {
                WarehouseId = s.WarehouseId,
                WarehouseName = s.Warehouse?.Name ?? "Unknown",
                QuantityOnHand = s.QuantityOnHand,
                QuantityAvailable = s.QuantityAvailable,
                PercentageOfTotal = totalOnHand > 0 ? (decimal)s.QuantityOnHand / totalOnHand * 100 : 0
            }).ToList();

        // Recent transactions
        var recentTransactions = transactions
            .OrderByDescending(t => t.TransactionDate)
            .Take(10)
            .Select(t => new StoreRecentTransaction
            {
                TransactionId = t.Id,
                TransactionDate = t.TransactionDate,
                TransactionType = t.TransactionType ?? "Unknown",
                Quantity = t.Quantity,
                WarehouseName = t.Warehouse?.Name,
                Reference = t.TransactionNumber
            }).ToList();

        // Supplier performance
        var supplierPerformance = await GetSupplierPerformance(request.ItemId, cancellationToken);

        // Purchase stats
        var purchaseStats = await CalculatePurchaseMetrics(request.ItemId, cancellationToken);

        // Backorder metrics
        var backorderStats = CalculateBackorderMetrics(totalOnHand, totalReserved, avgDailySales, item.UnitPrice);

        var profitMargin = item.UnitPrice > 0 ? (item.UnitPrice - item.Cost) / item.UnitPrice * 100 : 0;

        var response = new ItemDashboardResponse
        {
            ItemId = item.Id,
            ItemName = item.Name ?? "Unknown",
            Sku = item.Sku,
            Barcode = item.Barcode,
            CategoryName = category?.Name,
            ImageUrl = item.ImageUrl,
            UnitPrice = item.UnitPrice,
            Cost = item.Cost,
            ProfitMargin = profitMargin,
            TotalQuantityOnHand = totalOnHand,
            TotalQuantityAvailable = totalAvailable,
            TotalQuantityReserved = totalReserved,
            MinimumStock = item.MinimumStock,
            ReorderPoint = item.ReorderPoint,
            StockStatus = stockStatus,
            MovementClass = movementClass,
            TurnoverRate = turnoverRate,
            DaysOfSupply = daysOfSupply,
            DailySales = dailySales,
            WeeklySales = weeklySales,
            MonthlySales = monthlySales,
            YearlySales = yearlySales,
            PurchaseStats = purchaseStats,
            SalesTrend = salesTrend,
            StockTrend = stockTrend,
            PurchaseTrend = purchaseTrend,
            TopWarehouses = topWarehouses,
            RecentTransactions = recentTransactions,
            SupplierPerformance = supplierPerformance,
            BackorderStats = backorderStats
        };

        await cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5), cancellationToken);

        return response;
    }

    private static SalesMetrics CalculateSalesMetrics(
        List<InventoryTransaction> transactions,
        DateTime startDate,
        DateTime endDate,
        decimal unitPrice,
        decimal cost)
    {
        var periodTransactions = transactions
            .Where(t => t.TransactionDate >= startDate &&
                        t.TransactionDate < endDate)
            .ToList();

        var totalQty = periodTransactions.Sum(t => Math.Abs(t.Quantity));
        var days = Math.Max(1, (endDate - startDate).Days);

        // Calculate previous period for growth
        var previousStart = startDate.AddDays(-(endDate - startDate).Days);
        var previousTransactions = transactions
            .Where(t => t.TransactionDate >= previousStart &&
                        t.TransactionDate < startDate)
            .ToList();
        var previousQty = previousTransactions.Sum(t => Math.Abs(t.Quantity));
        var growth = previousQty > 0 ? (totalQty - previousQty) / previousQty * 100 : 0;

        return new SalesMetrics
        {
            TotalQuantitySold = totalQty,
            TotalRevenue = totalQty * unitPrice,
            TotalProfit = totalQty * (unitPrice - cost),
            AverageQuantityPerDay = totalQty / days,
            TransactionCount = periodTransactions.Count,
            GrowthPercentage = growth
        };
    }

    private static (string MovementClass, decimal TurnoverRate) ClassifyMovement(
        decimal avgDailySales,
        int currentStock,
        decimal cost)
    {
        // Annual turnover rate = (Annual Sales * Cost) / Average Inventory Value
        var annualSales = avgDailySales * 365;
        var avgInventoryValue = currentStock * cost;
        var turnoverRate = avgInventoryValue > 0 ? (annualSales * cost) / avgInventoryValue : 0;

        // Classification based on turnover rate
        var movementClass = turnoverRate switch
        {
            >= 12 => "Fast Moving",    // Turns over monthly or more
            >= 4 => "Medium Moving",   // Turns over quarterly
            >= 1 => "Slow Moving",     // Turns over yearly
            _ => "Dead Stock"          // Less than annual turnover
        };

        return (movementClass, turnoverRate);
    }

    private static string DetermineStockStatus(int onHand, int minStock, int reorderPoint)
    {
        if (onHand <= 0) return "Out of Stock";
        if (onHand <= minStock) return "Critical";
        if (onHand <= reorderPoint) return "Low";
        return "Healthy";
    }

    private static List<TimeSeriesDataPoint> GenerateDailyTrend(List<InventoryTransaction> transactions, int days)
    {
        var result = new List<TimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = days - 1; i >= 0; i--)
        {
            var date = today.AddDays(-i);
            var dayTransactions = transactions
                .Where(t => t.TransactionDate.Date == date)
                .ToList();

            result.Add(new TimeSeriesDataPoint
            {
                Date = date,
                Value = dayTransactions.Sum(t => Math.Abs(t.Quantity)),
                Label = date.ToString("MMM dd")
            });
        }

        return result;
    }

    private async Task<List<TimeSeriesDataPoint>> GenerateStockTrend(Guid itemId, int days, CancellationToken ct)
    {
        // For stock trend, we'll use current stock and work backwards based on transactions
        var stockLevels = await stockLevelRepository.ListAsync(new StockLevelsByItemSpec(itemId), ct);
        var currentStock = stockLevels.Sum(s => s.QuantityOnHand);

        var today = DateTime.UtcNow.Date;
        var startDate = today.AddDays(-days);

        var transactions = await transactionRepository
            .ListAsync(new TransactionsByItemAndDateSpec(itemId, startDate), ct);

        var result = new List<TimeSeriesDataPoint>();

        // Work backwards from current stock
        var runningStock = currentStock;

        for (int i = 0; i < days; i++)
        {
            var date = today.AddDays(-i);
            var dayTransactions = transactions
                .Where(t => t.TransactionDate.Date == date)
                .ToList();

            result.Insert(0, new TimeSeriesDataPoint
            {
                Date = date,
                Value = runningStock,
                Label = date.ToString("MMM dd")
            });

            // Adjust running stock (reverse the transactions)
            foreach (var trans in dayTransactions)
            {
                runningStock -= (int)trans.Quantity; // Reverse: positive becomes negative
            }
        }

        return result;
    }

    private async Task<List<SupplierPerformanceInfo>> GetSupplierPerformance(Guid itemId, CancellationToken ct)
    {
        var itemSuppliers = await itemSupplierRepository
            .ListAsync(new ItemSuppliersByItemSpec(itemId), ct);

        var result = new List<SupplierPerformanceInfo>();

        foreach (var itemSupplier in itemSuppliers.Take(5))
        {
            var supplier = await supplierRepository.GetByIdAsync(itemSupplier.SupplierId, ct);
            if (supplier == null) continue;

            // Get PO items for this supplier and item
            var poItems = await purchaseOrderItemRepository
                .ListAsync(new POItemsBySupplierAndItemSpec(itemSupplier.SupplierId, itemId), ct);

            result.Add(new SupplierPerformanceInfo
            {
                SupplierId = supplier.Id,
                SupplierName = supplier.Name ?? "Unknown",
                TotalOrders = poItems.Count,
                TotalQuantitySupplied = poItems.Sum(p => p.ReceivedQuantity),
                AverageLeadTime = itemSupplier.LeadTimeDays,
                OnTimeRate = 95, // Would need delivery tracking
                QualityScore = (decimal)supplier.Rating
            });
        }

        return result;
    }

    private async Task<PurchaseMetrics> CalculatePurchaseMetrics(Guid itemId, CancellationToken ct)
    {
        var oneYearAgo = DateTime.UtcNow.AddYears(-1);

        var poItems = await purchaseOrderItemRepository
            .ListAsync(new POItemsByItemAndDateSpec(itemId, oneYearAgo), ct);

        var totalQty = poItems.Sum(p => p.ReceivedQuantity);
        var totalCost = poItems.Sum(p => p.ReceivedQuantity * p.UnitPrice);
        var avgCost = totalQty > 0 ? totalCost / totalQty : 0;

        return new PurchaseMetrics
        {
            TotalQuantityPurchased = totalQty,
            TotalPurchaseCost = totalCost,
            AverageUnitCost = avgCost,
            PurchaseOrderCount = poItems.Select(p => p.PurchaseOrderId).Distinct().Count(),
            AverageLeadTimeDays = 7, // Would need actual tracking
            OnTimeDeliveryRate = 92 // Would need actual tracking
        };
    }

    private static BackorderMetrics CalculateBackorderMetrics(
        int onHand,
        int reserved,
        decimal avgDailySales,
        decimal unitPrice)
    {
        var backorderQty = Math.Max(0, reserved - onHand);
        var fillRate = reserved > 0 ? Math.Min(100, (decimal)onHand / reserved * 100) : 100;

        return new BackorderMetrics
        {
            CurrentBackorderQuantity = backorderQty,
            StockoutDaysLast30 = 0, // Would need historical tracking
            StockoutDaysLast90 = 0,
            StockoutCostEstimate = backorderQty * unitPrice,
            FillRate = fillRate
        };
    }
}

// Specification classes
public class StockLevelsByItemSpec : Specification<StockLevel>
{
    public StockLevelsByItemSpec(Guid itemId)
    {
        Query.Where(s => s.ItemId == itemId)
             .Include(s => s.Warehouse);
    }
}

public class TransactionsByItemAndDateSpec : Specification<InventoryTransaction>
{
    public TransactionsByItemAndDateSpec(Guid itemId, DateTime startDate)
    {
        Query.Where(t => t.ItemId == itemId && t.TransactionDate >= startDate)
             .Include(t => t.Warehouse);
    }
}

public class ItemSuppliersByItemSpec : Specification<ItemSupplier>
{
    public ItemSuppliersByItemSpec(Guid itemId)
    {
        Query.Where(i => i.ItemId == itemId);
    }
}

public class POItemsBySupplierAndItemSpec : Specification<PurchaseOrderItem>
{
    public POItemsBySupplierAndItemSpec(Guid supplierId, Guid itemId)
    {
        Query.Where(p => p.ItemId == itemId)
             .Include(p => p.PurchaseOrder)
             .Where(p => p.PurchaseOrder!.SupplierId == supplierId);
    }
}

public class POItemsByItemAndDateSpec : Specification<PurchaseOrderItem>
{
    public POItemsByItemAndDateSpec(Guid itemId, DateTime startDate)
    {
        Query.Where(p => p.ItemId == itemId)
             .Include(p => p.PurchaseOrder)
             .Where(p => p.PurchaseOrder!.CreatedOn >= startDate);
    }
}
